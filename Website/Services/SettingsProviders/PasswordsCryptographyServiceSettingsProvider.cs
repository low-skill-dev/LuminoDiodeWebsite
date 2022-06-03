using Microsoft.Extensions.Configuration;

namespace Website.Services.SettingsProviders
{
	/// <summary>
	/// This class is not currently in use.
	/// It's built into the system just in case.
	/// </summary>
	public class PasswordsCryptographyServiceSettingsProvider
	{
		protected readonly IConfiguration config;
		public PasswordsCryptographyServiceSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
		}

		public virtual int SaltSizeBytes
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("PasswordsCryptographyServiceSettings:SaltSize_bytes")
						.Get<int>();
				}
				catch
				{
					// default
					return 128;
				}
			}
		}
	}
}
