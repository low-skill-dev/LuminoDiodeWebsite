using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Website.Middleware
{
	public class ErrorPageGenerationMiddleware
	{
		private RequestDelegate next;

		public ErrorPageGenerationMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			await this.next.Invoke(context);

			var code = context.Response.StatusCode;
			if (code>=400 && code < 600)
			{
				context.Response.Clear();
				await context.Response.WriteAsync($"ERROR: HTTP {code}");
			}
		}
	}
}
