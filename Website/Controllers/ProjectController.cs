using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;

namespace Website.Controllers
{
	public class ProjectController : Controller
	{
		private readonly IServiceScopeFactory ScopeFactory;
		private readonly WebsiteContext context;
		public ProjectController(IServiceScopeFactory Services)
		{
			this.ScopeFactory = Services;
			this.context = Services.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>();
		}
		[HttpGet]
		public ViewResult Summary()
		{
			var loadedProjects = this.context.Projects.ToList();
			return this.View(loadedProjects);
		}
	}
}
