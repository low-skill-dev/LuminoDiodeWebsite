using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class ProjectsGroupController : Controller
	{
		[HttpGet]
		public ViewResult Summary()
		{
			return View();
		}
	}
}
