using Microsoft.Extensions.Configuration;

namespace Website.Services.SettingsProviders
{
	public class AppSettingsProvider
	{
		protected readonly IConfiguration config;
		public AppSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
			this.FrequentSearchRequestsServiceSP = new(configuration);
			this.RecentDocumentsBackgroundServiceSP = new(configuration);
			this.ConnectionStringsP = new(configuration);
		}

		public ConnectionStringsProvider ConnectionStringsP
		{ get; private set; }
		public FrequentSearchRequestsServiceSettingsProvider FrequentSearchRequestsServiceSP
		{ get; private set; }
		public RecentDocumentsBackgroundServiceSettingsProvider RecentDocumentsBackgroundServiceSP
		{ get; private set; }
	}
}
