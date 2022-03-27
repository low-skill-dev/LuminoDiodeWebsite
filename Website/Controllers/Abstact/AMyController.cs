using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;
using System.Collections.Generic;

namespace Website.Controllers
{
	/// <summary>
	/// Literally 'my controller'. A class that should be inherited by all controllers on this website.
	/// </summary>
	public abstract class AMyController : Controller
	{
		protected readonly WebsiteContext DbCtx;
		protected readonly SessionManager SM;
		protected readonly RequestsFromIpCounterService RC;
		protected Website.Models.UserModel.User? AuthedUser { get; set; } = null;
		protected readonly List<Website.Models.ViewModels.Alert> PageTopAlerts = new();
		public AMyController(IServiceScopeFactory ScopeFactory)
		{
			var sp = ScopeFactory.CreateScope().ServiceProvider;
			this.DbCtx = sp.GetRequiredService<WebsiteContext>();
			this.SM = sp.GetRequiredService<SessionManager>();
			this.RC = sp.GetRequiredService<RequestsFromIpCounterService>();
			ViewBag.AuthedUser = new Website.Models.UserModel.User { };
			ViewBag.AuthedUser = null;
			ViewBag.PageTopAlerts = this.PageTopAlerts;
		}
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			RC.CountRequest(context);
			if (RC.IPAddressIsBanned(context)) context.Result = new StatusCodeResult(429); // Return Http429 if too many requests
			base.OnActionExecuting(context);
			LoadSessionAndAuthedUser(); // Auth
		}
		protected void LoadSessionAndAuthedUser()
		{
			ViewBag.AuthedUser = null;
			if (Request.Cookies.ContainsKey(SessionManager.SessionIdCoockieName))
			{
				SessionInfo? Info;
#pragma warning disable CS8604
				SM.ValidateSession(Request.Cookies[SessionManager.SessionIdCoockieName], out Info);
#pragma warning restore CS8604
				this.AuthedUser = DbCtx.Users.Find(Info?.UserId) ?? null /* throw new System.AggregateException("Session cotains user id which cannot be found in DB")*/;
				ViewBag.AuthedUser = this.AuthedUser;
			}
		}
	}
}
