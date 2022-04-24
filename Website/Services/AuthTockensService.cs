using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Services.SettingsProviders;

namespace Website.Services
{

	public class AuthTockenService : BackgroundService
	{
		public Dictionary<string, (byte[] Key, DateTime CreatedDateTime)> AuthTockens { get; private set; } = new();

		private readonly AuthTockenServiceSettingsProvider SettingsProvider;
		private int TockenLifeTimeSecs
			=> this.SettingsProvider.TockenLifetime_secs;
		private int TockenIdStringLength
			=> this.SettingsProvider.TockenIdStringLength_chars;
		private int TokenKeyLengthBytes
			=> this.SettingsProvider.TokenKeyLength_bytes;
		private int TockensCleanUpIntervalSecs
			=> this.SettingsProvider.TokensCleanUpInterval_secs;

		public AuthTockenService(AppSettingsProvider SettingsProvider)
		{
			this.SettingsProvider = SettingsProvider.AuthTockenServiceSP;
		}
	}
}

