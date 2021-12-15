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
	public class ConnectionStringsProvider
	{
		protected readonly IConfiguration config;
		public ConnectionStringsProvider(IConfiguration configuration)
		{
			this.config = configuration;
		}

		public string? DefaultNpgsqlConnection
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("ConnectionStrings:DefaultNpgsqlConnection")
						.Get<string>();
				}
				catch (Exception)
				{
					return null;
				}
			}
		}
	}
}
