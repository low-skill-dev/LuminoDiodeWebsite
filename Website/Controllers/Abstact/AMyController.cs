using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	/// <summary>
	/// Literally 'my controller'. A class that should be inherited by all controllers on this website.
	/// </summary>
	public abstract class AMyController : Controller
	{
		protected readonly IServiceProvider ServiceProvider;
		protected readonly WebsiteContext context;
		protected readonly SessionManager SM;
		protected readonly RequestsFromIpCounterService RC;

		protected Website.Models.UserModel.User? AuthedUser { get; set; } = null;
		protected const string PageTopAlertsTempDataName = "PageTopAlerts";

		public AMyController(IServiceScopeFactory ScopeFactory)
		{
			var sp = ScopeFactory.CreateScope().ServiceProvider;
			this.ServiceProvider = sp;
			this.context = sp.GetRequiredService<WebsiteContext>();
			this.SM = sp.GetRequiredService<SessionManager>();
			this.RC = sp.GetRequiredService<RequestsFromIpCounterService>();
		}
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			this.ViewBag.AuthedUser = this.AuthedUser = (Models.UserModel.User?)this.HttpContext?.Items[Middleware.AuthenticationMiddleware.AuthedUsedObjectItemName] ?? null;
		}

		protected void AddAlertToPageTop(Website.Models.ViewModels.Alert alert)
		{
			if (!this.TempData.ContainsKey(PageTopAlertsTempDataName))
			{
				this.TempData.Add(PageTopAlertsTempDataName, string.Empty);
			}
			if (this.TempData.Peek(PageTopAlertsTempDataName) is not string)
			{
				this.TempData.Remove(PageTopAlertsTempDataName);
				this.TempData.Add(PageTopAlertsTempDataName, string.Empty);
			}

			var peekedStr = (string)this.TempData.Peek(PageTopAlertsTempDataName)!;
			var StringWithNewAlert = peekedStr + (string.IsNullOrEmpty(peekedStr) ? string.Empty : "\n") + alert.ToHtmlString();

			this.TempData.Remove(PageTopAlertsTempDataName);
			this.TempData.Add(PageTopAlertsTempDataName, StringWithNewAlert);
		}
	}
}