using Microsoft.Extensions.Configuration;
using System;

namespace Website.Services.SettingsProviders
{
	public class RequestsFromIpCounterServiceSettingsProvider
	{
		protected readonly IConfiguration config;
		public RequestsFromIpCounterServiceSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
		}

		public virtual int ControlledPeriod_secs // MOCKable
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("RequestsFromIpCounterServiceSettings:ControlledPeriod_secs")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 5;
				}
			}
		}
		public virtual int AllowedNumOfRequestsPerPeriod // MOCKable
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("RequestsFromIpCounterServiceSettings:AllowedNumOfRequestsPerPeriod")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 20;
				}
			}
		}
		public virtual int UnbanInterval_secs // MOCKable
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("RequestsFromIpCounterServiceSettings:UnbanInterval_secs")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 120;
				}
			}
		}
	}
}
