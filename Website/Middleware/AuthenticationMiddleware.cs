using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Website.Repository;
using Website.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Website.Middleware
{
	public class AuthenticationMiddleware
	{
		public const string AuthedUsedObjectItemName = "AuthedUser";

		private RequestDelegate next;
		private SessionManager SM;
		private WebsiteContext dbContext;
		public AuthenticationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
		{
			this.next = next;

			var sp = scopeFactory.CreateScope().ServiceProvider;
			this.SM = sp.GetRequiredService<SessionManager>();
			this.dbContext = sp.GetRequiredService<WebsiteContext>();
		}
		public async Task Invoke(HttpContext context)
		{
			await LoadSessionAndAuthedUser(context);
			await this.next.Invoke(context);
		}

		private async Task LoadSessionAndAuthedUser(HttpContext context)
		{
			if (context.Request.Cookies.ContainsKey(SessionManager.SessionIdCoockieName))
			{
				SessionInfo? Info = null;
				if (context.Request.Cookies.TryGetValue(SessionManager.SessionIdCoockieName, out var cookieVal))
					if (this.SM.ValidateSession(cookieVal!, out Info) == false) return;

				if (Info is null) return;

				context.Items.Add(AuthedUsedObjectItemName, await this.dbContext.Users.AsTracking(QueryTrackingBehavior.TrackAll).FirstOrDefaultAsync(u => u.Id == Info.UserId));
			}
		}
	}
}
