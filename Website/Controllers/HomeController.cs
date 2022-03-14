using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public class HomeController : AControllerWithAuth
	{
		private readonly IServiceScopeFactory ScopeFactory;
		private readonly WebsiteContext context;
		public HomeController(IServiceScopeFactory Services, SessionManager SM)
			: base(SM, Services.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>())
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
