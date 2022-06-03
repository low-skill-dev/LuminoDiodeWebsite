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

		public virtual int SessionLifetime_secs
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("SessionManagerSettings:SessionLifetime_secs")
						.Get<int>();
				}
				catch
				{
					// default
					return 86400;
				}
			}
		}
		public virtual int SessionIdStringLength_bytes
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("SessionManagerSettings:SessionIdStringLength_bytes")
						.Get<int>();
				}
				catch
				{
					// default
					return 100;
				}
			}
		}
		public virtual int SessionsCleanUpInterval_secs
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("SessionManagerSettings:SessionsCleanUpInterval_secs")
						.Get<int>();
				}
				catch
				{
					// default
					return 600;
				}
			}
		}
	}
}
