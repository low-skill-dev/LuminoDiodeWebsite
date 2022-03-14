using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public class ProjectController : AControllerWithAuth
	{
		private readonly IServiceScopeFactory ScopeFactory;
		private readonly WebsiteContext context;
		public ProjectController(IServiceScopeFactory Services, SessionManager SM)
			: base(SM, Services.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>())
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
