using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public abstract class AControllerWithAuth : Controller
	{
		protected Website.Models.UserModel.User? AuthedUser { get; set; } = null;
		private WebsiteContext DbCtx;
		//protected Website.Models.UserModel.User? AuthedUser=null;
		protected readonly SessionManager SM;
		public AControllerWithAuth(SessionManager SM, WebsiteContext ctx)
		{
			this.DbCtx = ctx;
			this.SM = SM;
			ViewBag.AuthedUser = null;
			/*
			if (HttpContext.Current.Request.Cookies.ContainsKey(SessionManager.SessionIdCoockieName))
			{
				SessionInfo? Info;
#pragma warning disable CS8604
				SM.ValidateSession(Request.Cookies[SessionManager.SessionIdCoockieName], out Info);
#pragma warning restore CS8604
				this.AuthedUserId = (Info?.UserId) ?? null;
			}
			*/
		}
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
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
