using Microsoft.AspNetCore.Mvc;
using Website.Models.DocumentModel;

namespace Website.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ViewResult Summary()
		{
			return View(model: new string[] { "Coding alpha" });
		}
	}
}
