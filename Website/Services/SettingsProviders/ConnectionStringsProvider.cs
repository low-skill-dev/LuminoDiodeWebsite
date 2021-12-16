using Microsoft.Extensions.Configuration;
using System;

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
