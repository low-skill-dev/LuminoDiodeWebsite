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

namespace Website.Services.SettingsProviders
{
	public class AppSettingsProvider
	{
		protected readonly IConfiguration config;
		public AppSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
			this.FrequentSearchRequestsServiceSP = new(configuration);
			this.RecentDocumentsBackgroundServiceSP = new(configuration);
			this.ConnectionStringsP = new(configuration);
		}

		public ConnectionStringsProvider ConnectionStringsP 
		{ get;private set; }
		public FrequentSearchRequestsServiceSettingsProvider FrequentSearchRequestsServiceSP
		{ get; private set; }
		public RecentDocumentsBackgroundServiceSettingsProvider RecentDocumentsBackgroundServiceSP
		{ get; private set; }
	}
}
