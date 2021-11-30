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
			return View(model: (this.context.DbDocuments.AsQueryable()));
		}
		public ViewResult Show1()
		{
			return View(freqServ.FrequentRequests.Select(x=> x.DocumentSearchServiceScope.Request + '\t'+x.Frequency.ToString()));
		}
	}
}
