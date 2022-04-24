using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Website.Services;
using System.Collections.Generic;

namespace Website.Controllers
{
	public sealed class DocumentController : AMyController
	{
		private readonly Website.Services.RecentDocumentsBackgroundService recentDocumentsProvider;
		public DocumentController(IServiceScopeFactory ScopeFactory)
			: base(ScopeFactory)
		{
			this.recentDocumentsProvider = base.ServiceProvider.GetRequiredService<RecentDocumentsBackgroundService>();
			base.ServiceProvider.GetRequiredService<RandomDataSeederService>().SeedData();
		}

		[HttpGet]
		public ViewResult Summary()
		{
			return this.View(this.recentDocumentsProvider.RecentDocuments);
		}

		[HttpGet]
		public async Task<IActionResult> Show(int Id)
		{
			var loadedDoc = await this.context.DbDocuments.Include("Author").FirstOrDefaultAsync(d => d.Id == Id);
			if (loadedDoc == null) return new StatusCodeResult(404);

			this.ViewBag.AutherUserIsOwner = this.AuthedUser?.Id.Equals(loadedDoc.Author?.Id) ?? false;

			return this.View(loadedDoc.ToDocument());
		}


		[HttpGet]
		public IActionResult Search()
		{
			return this.View(new List<Website.Models.DocumentModel.Document>());
		}
		[HttpPost]
		public async Task<ViewResult> Search(string SearchRequest)
		{
			this.ViewBag.SearchRequest = SearchRequest;
			return this.View((await base.ServiceProvider.GetRequiredService<DocumentSearchService>().ProceedRequest(SearchRequest)).Select(x => x.ToDocument()));
		}

		[HttpGet]
		public IActionResult Create()
		{
			if (this.AuthedUser is null)
				return new StatusCodeResult(401); // 401 Unauthorized

			return this.View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(Website.Models.DocumentModel.DocumentCreation Doc)
		{
			if (this.AuthedUser is null)
				return new StatusCodeResult(401); // 401 Unauthorized

			if (!this.ModelState.IsValid)
				return this.View(Doc);

			var DocForAddingToDb = new Website.Models.DocumentModel.Document
			{
				Title = Doc.Title,
				Author = this.context.Users.Find(this.AuthedUser.Id),
				CreatedDateTime = System.DateTime.UtcNow,
				Paragraphs = new Models.DocumentModel.DocumentParagraph[] { new Models.DocumentModel.DocumentParagraph {
						TextParts= new Website.Models.DocumentModel.WebText[] {new Models.DocumentModel.WebText { Text=Doc.Text} } } }
			};

			this.context.DbDocuments.Add(Website.Models.DocumentModel.DbDocument.FromDocument(DocForAddingToDb));
			await this.context.SaveChangesAsync();

			return this.RedirectToAction("Show", new { Id = DocForAddingToDb.Id });
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int Id)
		{
			if (this.AuthedUser is null)
				return new StatusCodeResult(401);

			var FoundDoc = await this.context.DbDocuments.Include("Author").SingleOrDefaultAsync(d => d.Id == Id);

			if (FoundDoc is null)
				return new StatusCodeResult(404);

			if ((!FoundDoc.Author?.Id.Equals(this.AuthedUser.Id)) ?? true)
				return new StatusCodeResult(401);

			return this.View("Edit", Website.Models.DocumentModel.DocumentEdition.FromDocument(FoundDoc.ToDocument()));
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Website.Models.DocumentModel.DocumentCreation Doc)
		{
			// User not authorized
			if (this.AuthedUser is null)
				return new StatusCodeResult(401);

			// Bad request. No Id for editing passed.
			var PassedId = this.RouteData.Values["Id"];
			if (PassedId is null) 
				return new StatusCodeResult(400);

			// Bad request. Id cannot be parsed. (possible NaN)
			int ParsedId;
			if (!int.TryParse(PassedId as string, out ParsedId)) 
				return new StatusCodeResult(400);

			// Not found. Server understood request and parsed id, but not such id was found in Db.
			var FoundDoc = await this.context.DbDocuments.Include("Author").FirstOrDefaultAsync(x=> x.Id==ParsedId);
			if (FoundDoc is null)
				return new StatusCodeResult(404);

			// Authed user is not owner of the passed document. Forbidden.
			var DocumentOwnerId = FoundDoc.Author!.Id;
			if (AuthedUser.Id != DocumentOwnerId)
				return new StatusCodeResult(403);
		
			// Passed text or title is not valid
			if (!this.ModelState.IsValid)
				return this.View(Doc);

			// Creating new document for replace
			var DocForAddingToDb = new Website.Models.DocumentModel.Document
			{
				Title = Doc.Title,
				Author = FoundDoc.Author,
				CreatedDateTime = FoundDoc.CreatedDateTime,
				Paragraphs = new Models.DocumentModel.DocumentParagraph[] { new Models.DocumentModel.DocumentParagraph {
						TextParts= new Website.Models.DocumentModel.WebText[] {new Models.DocumentModel.WebText { Text=Doc.Text} } } }
			};

			// Replacing entity for EF
			FoundDoc = Website.Models.DocumentModel.DbDocument.FromDocument(DocForAddingToDb);

			// Saving changes async
			await this.context.SaveChangesAsync();

			// returning View for edited document
			return this.View("Show", FoundDoc.ToDocument());
		}

		[HttpGet]
		public ViewResult Delete(int ProjectId)
		{
			return this.View();
		}
		[HttpDelete]
		public StatusCodeResult Delete(int ProjectId, int UserPerformsActionId)
		{
			return new StatusCodeResult(202);
		}
	}
}
