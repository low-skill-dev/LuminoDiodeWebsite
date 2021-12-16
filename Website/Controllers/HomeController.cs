using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ViewResult Summary()
		{
			return this.View(model: new string[] { "Coding alpha" });
		}
	}
}
