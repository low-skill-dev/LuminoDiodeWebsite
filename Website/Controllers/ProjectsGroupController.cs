using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Website.Repository;
using Website.Services;


namespace Website.Controllers
{
	public class ProjectsGroupController : AMyController
	{
		private readonly IServiceScopeFactory ScopeFactory;
		private readonly Website.Repository.WebsiteContext context;
		private readonly Website.Services.RecentDocumentsBackgroundService recentDocumentsProvider;
		public ProjectsGroupController(IServiceScopeFactory Services, Website.Services.RecentDocumentsBackgroundService documentsBackgroundService, SessionManager SM)
			: base(Services)
		{
			this.ScopeFactory = Services;
			this.context = Services.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>();
			this.recentDocumentsProvider = documentsBackgroundService;
		}
		[HttpGet]
		public ViewResult Summary()
		{
			return this.View();
		}
	}
}
