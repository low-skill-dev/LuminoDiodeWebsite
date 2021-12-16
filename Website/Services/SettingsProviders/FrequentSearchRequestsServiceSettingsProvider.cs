using Microsoft.Extensions.Configuration;
using System;

namespace Website.Services.SettingsProviders
{
	public class FrequentSearchRequestsServiceSettingsProvider
	{
		protected readonly IConfiguration config;
		public FrequentSearchRequestsServiceSettingsProvider(IConfiguration configuration)
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
						.GetRequiredSection("FrequentSearchRequestsServiceSettings:Interval_msec")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 300000;
				}
			}
		}
		public int Interval_numOfRecentRequests
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("FrequentSearchRequestsServiceSettings:Interval_numOfRecentRequests")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 50;
				}
			}
		}
		public int Interval_updateNeededCheck_msec
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("FrequentSearchRequestsServiceSettings:Interval_updateNeededCheck_msec")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 10000;
				}
			}
		}
		public int NumOfFrequentRequestsStored
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("FrequentSearchRequestsServiceSettings:NumOfFrequentRequestsStored")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 50;
				}
			}
		}
		public int TokenSortRationNeededToCountAsSimilar
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("FrequentSearchRequestsServiceSettings:TokenSortRationNeededToCountAsSimilar")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 90;
				}
			}
		}
		public int ResponseLifetime_msec
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("FrequentSearchRequestsServiceSettings:ResponseLifetime_msec")
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
