using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public sealed class ProjectController : AMyController
	{
		public ProjectController(IServiceScopeFactory Services)
			: base(Services)
		{
			
		}
		[HttpGet]
		public ViewResult Summary()
		{
			var loadedProjects = this.context.Projects.ToList();
			return this.View(loadedProjects);
		}
	}
}
