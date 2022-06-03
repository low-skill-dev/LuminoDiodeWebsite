using Microsoft.Extensions.Configuration;
using System;

namespace Website.Services.SettingsProviders
{
	public class AuthTockenServiceSettingsProvider
	{
		protected readonly IConfiguration config;
		public AuthTockenServiceSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
		}

		public virtual int TockenIdStringLength_chars
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("AuthTockensServiceSettings:TockenIdStringLength_chars")
						.Get<int>();
				}
				catch
				{
					// default
					return 100;
				}
			}
		}
		public virtual int TockenLifetime_secs
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("AuthTockensServiceSettings:TockenLifetime_secs")
						.Get<int>();
				}
				catch
				{
					// default
					return 600;
				}
			}
		}
		public virtual int TokenKeyLength_bytes
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("AuthTockensServiceSettings:TokenKeyLength_bytes")
						.Get<int>();
				}
				catch
				{
					// default
					return 64;
				}
			}
		}
		public virtual int TokensCleanUpInterval_secs
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("AuthTockensServiceSettings:TokensCleanUpInterval_secs")
						.Get<int>();
				}
				catch
				{
					// default
					return 1200;
				}
			}
		}

	}
}