using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Website.Services;
using static Microsoft.Extensions.DependencyInjection.NpgsqlServiceCollectionExtensions;

namespace Website.Controllers
{
	public class ProjectController : Controller
	{
		private IServiceScopeFactory ScopeFactory;
		private WebsiteContext context;
		public ProjectController(IServiceScopeFactory Services)
		{
			this.ScopeFactory = Services;
			this.context = Services.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>();
		}
		[HttpGet]
		public ViewResult Summary()
		{
			var loadedProjects = this.context.Projects.ToList();
			return View(loadedProjects);
		}
	}
}
