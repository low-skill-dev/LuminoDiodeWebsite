using Microsoft.Extensions.Configuration;

namespace Website.Services.SettingsProviders
{
	/// <summary>
	/// A <b>singleton</b> service which contains settings providers for services
	/// of the application as readonly properties.
	/// When creating, it creates all contained settings providers,
	/// which instantly reads values from appsettings.json, secrets.json etc and saves it to memory.
	/// </summary>
	public class AppSettingsProvider
	{
		protected readonly IConfiguration config;
		public AppSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
			this.FrequentSearchRequestsServiceSP = new(configuration);
			this.RecentDocumentsBackgroundServiceSP = new(configuration);
			this.ConnectionStringsP = new(configuration);
			this.PasswordsCryptographyServiceSP = new(configuration);
			this.SessionManagerServiceSP=new(configuration);
			this.RandomDataSeederSP = new(configuration);
			this.RequestsFromIpCounterServiceSP = new(configuration);
		}

		public ConnectionStringsProvider ConnectionStringsP
		{ get; private set; }
		public FrequentSearchRequestsServiceSettingsProvider FrequentSearchRequestsServiceSP
		{ get; private set; }
		public RecentDocumentsBackgroundServiceSettingsProvider RecentDocumentsBackgroundServiceSP
		{ get; private set; }
		public PasswordsCryptographyServiceSettingsProvider PasswordsCryptographyServiceSP
		{ get; private set; }
		public SessionManagerServiceSettingsProvider SessionManagerServiceSP
		{ get; private set; }
		public RandomDataSeederSettingsProvider RandomDataSeederSP
		{ get; private set; }
		public RequestsFromIpCounterServiceSettingsProvider RequestsFromIpCounterServiceSP
		{ get; private set; }
	}
}
