using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;
using System.Collections.Generic;
using System;

namespace Website.Controllers
{
	/// <summary>
	/// Literally 'my controller'. A class that should be inherited by all controllers on this website.
	/// </summary>
	public abstract class AMyController : Controller
	{
		protected readonly WebsiteContext context;
		protected readonly SessionManager SM;
		protected readonly RequestsFromIpCounterService RC;

		protected Website.Models.UserModel.User? AuthedUser { get; set; } = null;
		protected readonly List<Website.Models.ViewModels.Alert> PageTopAlerts = new(1);
		public AMyController(IServiceScopeFactory ScopeFactory)
		{
			var sp = ScopeFactory.CreateScope().ServiceProvider;
			this.context = sp.GetRequiredService<WebsiteContext>();
			this.SM = sp.GetRequiredService<SessionManager>();
			this.RC = sp.GetRequiredService<RequestsFromIpCounterService>();
			ViewBag.AuthedUser = new Website.Models.UserModel.User { };
			ViewBag.AuthedUser = null;

		}
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// Returns Http 429 if too many requests
			RC.CountRequest(context);
			if (RC.IPAddressIsBanned(context)) context.Result = new StatusCodeResult(429);

			base.OnActionExecuting(context);

			LoadSessionAndAuthedUser();
			RedirectToRegistrationStepIfNeeded(context);
		}
		protected void LoadSessionAndAuthedUser()
		{
			if (Request.Cookies.ContainsKey(SessionManager.SessionIdCoockieName))
			{
				SessionInfo? Info;
#pragma warning disable CS8604
				SM.ValidateSession(Request.Cookies[SessionManager.SessionIdCoockieName], out Info);
#pragma warning restore CS8604
				this.AuthedUser = context.Users.Find(Info?.UserId) ?? null /* throw new System.AggregateException("Session cotains user id which cannot be found in DB")*/;
				ViewBag.AuthedUser = this.AuthedUser;
			}
		}
		protected void AddAlertToPageTop(Website.Models.ViewModels.Alert alert)
		{
			if (this.TempData["PageTopAlerts"] == null)
				this.TempData["PageTopAlerts"] = this.PageTopAlerts;

			this.PageTopAlerts.Add(alert);
		}
		protected void RedirectToRegistrationStepIfNeeded(ActionExecutingContext context)
		{
			if (this.AuthedUser != null && AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.RegistrationCompleted)
			{
				var act = context.RouteData.Values["Action"];
				var ctr = context.RouteData.Values["controller"];

				if (AuthedUser.RegistrationStage == Models.UserModel.User.REGISTRATION_STAGE.EnteringName)
				{
					if (act is null || ctr is null || !act.Equals("RegistrationStageEnteringName") || !ctr.Equals("User"))
						context.Result = RedirectToAction("RegistrationStageEnteringName", "User");
				}
				if (AuthedUser.RegistrationStage == Models.UserModel.User.REGISTRATION_STAGE.EnteringMetadata)
				{
					if (act is null || ctr is null || !act.Equals("RegistrationStageEnteringMetadata") || !ctr.Equals("User"))
						context.Result = RedirectToAction("RegistrationStageEnteringMetadata", "User");
				}

			}
		}
	}
}
