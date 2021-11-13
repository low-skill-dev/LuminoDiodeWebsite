using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class DbgController : Controller
	{
		private readonly Website.Repository.WebsiteContext context;

		public DbgController(Website.Repository.WebsiteContext ctx)
		{
			this.context = ctx;
		}

		public ViewResult Summary()
		{
			return View(model: (this.context.DbArticles.AsQueryable()));
		}
	}
}
