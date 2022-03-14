using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
	public class DbgController : AControllerWithAuth
	{
		private readonly Website.Repository.WebsiteContext context;
		private readonly Website.Services.FrequentSearchRequestsService freqServ;
		public DbgController(Website.Repository.WebsiteContext ctx, Website.Services.FrequentSearchRequestsService freqServ, SessionManager SM) : base(SM, ctx)
		{
			this.context = ctx;
			this.freqServ = freqServ;
			//if(Request.Cookies.ContainsKey("SessionId"));
			ViewBag["UserName"] = 123;
		}

		public ViewResult Summary()
		{
			return this.View(model: (this.context.DbDocuments.AsQueryable()));
		}
		public ViewResult Show1()
		{
			return this.View(this.freqServ.FrequentRequests.Select(x => x.DocumentSearchServiceScope.Request + '\t' + x.Frequency.ToString()));
		}
	}
}
