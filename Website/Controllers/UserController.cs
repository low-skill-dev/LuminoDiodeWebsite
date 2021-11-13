using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
