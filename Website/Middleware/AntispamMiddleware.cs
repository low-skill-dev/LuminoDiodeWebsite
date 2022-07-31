using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Website.Repository;
using Website.Services;

namespace Website.Middleware
{
	public class AntispamMiddleware
	{
		private RequestDelegate next;
		private RequestsFromIpCounterService antispamService;

		public AntispamMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
		{
			this.next = next;

			var sp=scopeFactory.CreateScope().ServiceProvider;
			this.antispamService = sp.GetRequiredService<RequestsFromIpCounterService>();
		}
		public async Task Invoke(HttpContext context)
		{
			if(context.Connection.RemoteIpAddress is null)
			{
				await this.next.Invoke(context);
				return;
			}

			this.antispamService.CountRequest(context.Connection.RemoteIpAddress);

			if (this.antispamService.IPAddressIsBanned(context.Connection.RemoteIpAddress)) 
				context.Response.StatusCode = 429;
			else 
				 await this.next.Invoke(context);
		}
	}
}
