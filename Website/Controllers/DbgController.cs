using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public class DbgController : Controller
	{
		private readonly WebsiteContext context;
		private readonly FrequentSearchRequestsService freqServ;
		private readonly AppSettingsProvider configuration;
		public DbgController(Website.Repository.WebsiteContext ctx, Website.Services.FrequentSearchRequestsService freqServ, AppSettingsProvider _config)
		{
			this.context = ctx;
			this.freqServ = freqServ;
			this.configuration = _config;
		}

		public ViewResult Summary()
		{
			return this.View(model: (this.context.DbDocuments.AsQueryable()));
		}
		public ViewResult Show1()
		{
			return this.View(this.freqServ.FrequentRequests.Select(x => x.DocumentSearchServiceScope.Request + '\t' + x.Frequency.ToString()));
		}
		public ViewResult Show2()
		{
			return this.View(model: new string[] {new string(
				nameof(this.configuration.FrequentSearchRequestsServiceSP.Interval_msec) + " : "
				+ this.configuration.FrequentSearchRequestsServiceSP.Interval_msec.ToString()+ "\n"+
				nameof(this.configuration.FrequentSearchRequestsServiceSP.Interval_numOfRecentRequests) + " : "
				+ this.configuration.FrequentSearchRequestsServiceSP.Interval_numOfRecentRequests.ToString() + "\n" +
				nameof(this.configuration.FrequentSearchRequestsServiceSP.Interval_updateNeededCheck_msec) + " : "
				+ this.configuration.FrequentSearchRequestsServiceSP.Interval_updateNeededCheck_msec + "\n") });
		}
	}
}
