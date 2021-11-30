using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class ProjectController : Controller
	{
		[HttpGet]
		public ViewResult Summary()
		{
			return View();
		}
	}
}
