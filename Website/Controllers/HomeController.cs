using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Website.Controllers
{
	public class HomeController:Controller
	{
		[HttpGet]
		public ViewResult Index()
		{
			return View("Index", (new string[] { "Planning" }));

		}
	}
}
