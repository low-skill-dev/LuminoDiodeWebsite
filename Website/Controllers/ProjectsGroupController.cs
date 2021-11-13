using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class ProjectsGroupController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
