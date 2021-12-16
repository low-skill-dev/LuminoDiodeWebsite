using Microsoft.Extensions.Configuration;
using System;

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
