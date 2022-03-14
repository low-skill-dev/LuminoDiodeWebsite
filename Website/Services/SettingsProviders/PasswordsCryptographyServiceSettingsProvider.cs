using Microsoft.Extensions.Configuration;
using System;

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
	}
}
