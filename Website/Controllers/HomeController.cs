using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public class HomeController : AMyController
	{
		private readonly IServiceScopeFactory ScopeFactory;
		private readonly WebsiteContext context;
		public HomeController(IServiceScopeFactory Services, SessionManager SM)
			: base(Services)
		{
			this.ScopeFactory = Services;
			this.context = Services.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>();
		}

		[HttpGet]
		public ViewResult Summary()
		{
			return this.View(model: new string[] { "Coding alpha" });
		}
	}
}
