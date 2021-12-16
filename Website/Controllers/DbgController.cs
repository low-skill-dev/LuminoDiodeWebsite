using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using Website.Models.DocumentModel;
using Microsoft.Extensions.Configuration;
using Website.Services;
using Website.Services.SettingsProviders;

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
			return View(model: (this.context.DbDocuments.AsQueryable()));
		}
		public ViewResult Show1()
		{
			return View(freqServ.FrequentRequests.Select(x=> x.DocumentSearchServiceScope.Request + '\t'+x.Frequency.ToString()));
		}
		public ViewResult Show2()
		{
			return View(model:new string[] {new string(
				nameof(configuration.FrequentSearchRequestsServiceSP.Interval_msec) + " : "
				+ configuration.FrequentSearchRequestsServiceSP.Interval_msec.ToString()+ "\n"+
				nameof(configuration.FrequentSearchRequestsServiceSP.Interval_numOfRecentRequests) + " : "
				+ configuration.FrequentSearchRequestsServiceSP.Interval_numOfRecentRequests.ToString() + "\n" +
				nameof(configuration.FrequentSearchRequestsServiceSP.Interval_updateNeededCheck_msec) + " : "
				+ configuration.FrequentSearchRequestsServiceSP.Interval_updateNeededCheck_msec + "\n") });
		}
	}
}
