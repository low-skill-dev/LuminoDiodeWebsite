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

		public int TockenIdStringLength_chars
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("AuthTockensServiceSettings:TockenIdStringLength_chars")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 100;
				}
			}
		}
		public int TockenLifetime_secs
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("AuthTockensServiceSettings:TockenLifetime_secs")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 600;
				}
			}
		}
		public int TokenKeyLength_bytes
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("AuthTockensServiceSettings:TokenKeyLength_bytes")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 64;
				}
			}
		}
		public int TokensCleanUpInterval_secs
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("AuthTockensServiceSettings:TokensCleanUpInterval_secs")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 1200;
				}
			}
		}

	}
}