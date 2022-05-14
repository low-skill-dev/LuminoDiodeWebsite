using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Website.Repository;
using Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Website.Models.ViewModels;
using Website.Services;
using System.Linq;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace Website.Controllers
{
	[Obsolete]
	public sealed class ProjectsGroupController : AMyController
	{
		private readonly Website.Services.RecentDocumentsBackgroundService recentDocumentsProvider;
		public ProjectsGroupController(IServiceScopeFactory Services, Website.Services.RecentDocumentsBackgroundService documentsBackgroundService, SessionManager SM)
			: base(Services)
		{
			this.recentDocumentsProvider = documentsBackgroundService;
		}
		[HttpGet]
		public ViewResult Summary()
		{
			return this.View();
		}
	}
}
