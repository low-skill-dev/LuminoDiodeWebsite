﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public class DbgController : AMyController
	{
		private readonly Website.Repository.WebsiteContext context;
		private readonly Website.Services.FrequentSearchRequestsService freqServ;
		public DbgController(IServiceScopeFactory ScopeFactory) : base(ScopeFactory)
		{
			var sp = ScopeFactory.CreateScope().ServiceProvider;
			this.context = sp.GetRequiredService<WebsiteContext>();
			this.freqServ = sp.GetRequiredService<FrequentSearchRequestsService>();
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
