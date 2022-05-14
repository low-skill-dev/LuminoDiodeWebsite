using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public sealed class HomeController : AMyController
	{
		public HomeController(IServiceScopeFactory Services)
			: base(Services)
		{

		}

		[HttpGet]
		public ViewResult Summary()
		{
			return this.View(model: new string[] { "Coding alpha" });
		}
	}
}
