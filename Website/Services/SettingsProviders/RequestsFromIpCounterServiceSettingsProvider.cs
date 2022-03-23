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

		public int ControlledTime_mins
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("RequestsFromIpCounterServiceSettings:ControlledTime_mins")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 5;
				}
			}
		}
		public int AllowedNumOfRequestsPerMinute
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("RequestsFromIpCounterServiceSettings:AllowedNumOfRequestsPerMinute")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 20;
				}
			}
		}
	}
}
