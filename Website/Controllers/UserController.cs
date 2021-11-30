using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class UserController : Controller
	{
		[HttpGet]
		public ViewResult Summary()
		{
			return View();
		}
	}
}
