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
	public class RecentDocumentsBackgroundServiceSettingsProvider  
	{
		protected readonly IConfiguration config;
		public RecentDocumentsBackgroundServiceSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
		}

		public int Interval_msec
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("RecentDocumentsBackgroundServiceSettings:Interval_msec")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 300000;
				}
			}
		}
	}
}
