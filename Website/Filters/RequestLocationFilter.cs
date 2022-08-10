using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Website.Filters
{
	public class RequestLocationFilter : Attribute, IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.HttpContext.Connection.RemoteIpAddress?.AddressFamily == System.Net.Sockets.AddressFamily.Unix)
			{
				context.HttpContext.RequestAborted = new System.Threading.CancellationToken(true);
			}
			else
			{
				await next.Invoke();
			}
		}

	}
}
