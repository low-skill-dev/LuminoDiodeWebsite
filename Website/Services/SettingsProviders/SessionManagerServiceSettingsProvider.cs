using Microsoft.Extensions.Configuration;
using System;

namespace Website.Services.SettingsProviders
{
	public class SessionManagerServiceSettingsProvider
	{
		protected readonly IConfiguration config;
		public SessionManagerServiceSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
		}

		public int SessionLifetime_secs
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("SessionManagerSettings:SessionLifetime_secs")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 86400;
				}
			}
		}
		public int SessionIdStringLength_bytes
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("SessionManagerSettings:SessionIdStringLength_bytes")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 100;
				}
			}
		}
		public int SessionsCleanUpInterval_secs
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("SessionManagerSettings:SessionsCleanUpInterval_secs")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 600;
				}
			}
		}
	}
}
