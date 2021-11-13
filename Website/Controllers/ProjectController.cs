using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class ProjectController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
