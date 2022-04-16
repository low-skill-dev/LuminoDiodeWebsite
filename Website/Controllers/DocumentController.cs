using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Website.Services;

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
			return this.View(this.recentDocumentsProvider.RecentDocuments.Select(x => x.ToDocument()));
		}

		[HttpGet]
		public async Task<IActionResult> Show(int Id)
		{
			var loadedDoc = await this.context.DbDocuments.Include("Author").SingleOrDefaultAsync(d => d.Id == Id);
			if (loadedDoc == null) return new StatusCodeResult(404);

			this.ViewBag.AutherUserIsOwner = this.AuthedUser?.Id.Equals(loadedDoc.Author?.Id) ?? false;

			var v = loadedDoc.ToDocument();
			return this.View(v);
		}

		[HttpPost]
		public async Task<ViewResult> Search(string SearchRequest)
		{
			this.ViewBag.SearchRequest = SearchRequest;

			var ServiceProvider = base.ServiceProvider;
			var SearchService = ServiceProvider.GetRequiredService<DocumentSearchService>();
			var ProceedRes = await SearchService.ProceedRequest(SearchRequest);
			var ResResult = ProceedRes;
			var Loaded = ResResult.Take(10);
			return this.View(Loaded.Select(x => x.ToDocument()));
		}

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
			if (this.AuthedUser is null)
				return new StatusCodeResult(401); // 401 Unauthorized

			return this.View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Website.Models.DocumentModel.DocumentCreation Doc/*tobedefinied*/)
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
		#endregion

		#region Edit
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
			if (this.AuthedUser is null)
				return new StatusCodeResult(401); // 401 Unauthorized


			Website.Models.DocumentModel.DbDocument? FoundDoc = null;
			if (this.RouteData.Values["Id"] is not null)
			{
				try
				{
					FoundDoc = await this.context.DbDocuments.FindAsync(int.Parse(this.RouteData.Values["Id"] as string));
				}
				catch (ArgumentException)
				{

				}
			}

			if (FoundDoc is null)
				return new StatusCodeResult(404);

			if (!this.ModelState.IsValid)
				return this.View(Doc);

			var DocForAddingToDb = new Website.Models.DocumentModel.Document
			{
				Title = Doc.Title,
				Author = FoundDoc.Author,
				CreatedDateTime = FoundDoc.CreatedDateTime,
				Paragraphs = new Models.DocumentModel.DocumentParagraph[] { new Models.DocumentModel.DocumentParagraph {
						TextParts= new Website.Models.DocumentModel.WebText[] {new Models.DocumentModel.WebText { Text=Doc.Text} } } }
			};

			FoundDoc = Website.Models.DocumentModel.DbDocument.FromDocument(DocForAddingToDb);

			await this.context.SaveChangesAsync();

			return this.View("Show", FoundDoc.ToDocument());
		}
		#endregion

		#region Delete
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
		#endregion



	}
}
