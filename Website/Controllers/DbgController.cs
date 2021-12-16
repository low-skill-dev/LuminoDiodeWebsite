using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Website.Controllers
{
	public class DbgController : Controller
	{
		private readonly Website.Repository.WebsiteContext context;
		private readonly Website.Services.FrequentSearchRequestsService freqServ;
		public DbgController(Website.Repository.WebsiteContext ctx, Website.Services.FrequentSearchRequestsService freqServ)
		{
			this.context = ctx;
			this.freqServ = freqServ;
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
