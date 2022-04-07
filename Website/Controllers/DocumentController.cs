using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;
using System.Threading.Tasks;

namespace Website.Controllers
{
	public class DocumentController : AMyController
	{
		private readonly IServiceScopeFactory ScopeFactory;
		private readonly Website.Repository.WebsiteContext context;
		private readonly Website.Services.RecentDocumentsBackgroundService recentDocumentsProvider;
		public DocumentController(IServiceScopeFactory ScopeFactory)
			:base(ScopeFactory)
		{
			var sp = ScopeFactory.CreateScope().ServiceProvider;
			this.ScopeFactory = ScopeFactory;
			this.context = sp.GetRequiredService<WebsiteContext>();
			this.recentDocumentsProvider = sp.GetRequiredService<RecentDocumentsBackgroundService>();
			sp.GetRequiredService<RandomDataSeederService>().SeedData();
		}

		[HttpGet]
		public ViewResult Summary()
		{
			return this.View(this.recentDocumentsProvider.RecentDocuments.Select(x=>x.ToDocument()));
		}

		[HttpGet]
		public IActionResult Show(int Id)
		{
			var loadedDoc = this.context.DbDocuments.Include("Author").First();
			if (loadedDoc == null) return new StatusCodeResult(404);

			return this.View(loadedDoc.ToDocument());
		}

		[HttpPost]
		public async Task<ViewResult> Search(string SearchRequest)
		{
			this.ViewBag.SearchRequest = SearchRequest;

			var ServiceProvider = this.ScopeFactory.CreateScope().ServiceProvider;
			var SearchService = ServiceProvider.GetRequiredService<DocumentSearchService>();
			var ProceedRes = await SearchService.ProceedRequest(SearchRequest);
			var ResResult = ProceedRes;
			var Loaded = ResResult.Take(10);
			return this.View(Loaded.Select(x=> x.ToDocument()));
		}

		#region Create
		[HttpGet]
		public async Task<IActionResult> Create()
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

			if (!ModelState.IsValid)
				View(Doc);

			var DocForAddingToDb = new Website.Models.DocumentModel.Document
			{
				Title = Doc.Title,
				Author = context.Users.Find(this.AuthedUser.Id),
				CreatedDateTime = System.DateTime.UtcNow,
				Paragraphs = new Models.DocumentModel.DocumentParagraph[] { new Models.DocumentModel.DocumentParagraph {
						TextParts= new Website.Models.DocumentModel.WebText[] {new Models.DocumentModel.WebText { Text=Doc.Text} } } }
			};

			context.DbDocuments.Add(Website.Models.DocumentModel.DbDocument.FromDocument(DocForAddingToDb));
			await context.SaveChangesAsync();

			return RedirectToAction("Show", new { Id = DocForAddingToDb.Id });
		}
		#endregion

		#region Edit
		[HttpGet]
		public ViewResult Edit(int ProjectId)
		{
			return this.View();
		}

		[HttpPut]
		public StatusCodeResult Edit(int ProjectId, object DocumentToBePosted)
		{
			return new StatusCodeResult(202);
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
