using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Services.SettingsProviders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Cryptography;
using Website.Services.SettingsProviders;

namespace Website.Services
{
	public class TockenInfo
	{
		public byte[] HashKey { get; private set; }
		public DateTime ValidThrough { get; private set; }
		public int UserId { get; private set; }

		public TockenInfo(byte[] HashKey, DateTime ValidThrough, int UserId)
		{
			this.HashKey = HashKey; this.ValidThrough = ValidThrough; this.UserId = UserId;
		}
	}
	public class AuthTockenService : BackgroundService
	{
		public Func<byte[], byte[]> HashAlg = SHA512.HashData;

		private Dictionary<string, TockenInfo> AuthTockens = new();
		public bool TockenExists(string TockenId) => this.AuthTockens.ContainsKey(TockenId);
		public bool TryGetTocken(string TockenId, out TockenInfo Tocken) => this.AuthTockens.TryGetValue(TockenId, out Tocken!);

		private readonly AuthTockenServiceSettingsProvider SettingsProvider;
		private int TockenLifeTimeSecs => this.SettingsProvider.TockenLifetime_secs;
		private int TockenIdStringLength => this.SettingsProvider.TockenIdStringLength_chars;
		private int TokenKeyLengthBytes => this.SettingsProvider.TokenKeyLength_bytes;
		private int TockensCleanUpIntervalSecs => this.SettingsProvider.TokensCleanUpInterval_secs;

		public AuthTockenService(AppSettingsProvider SettingsProvider)
			=> this.SettingsProvider = SettingsProvider.AuthTockenServiceSP;

		public void CreateTocken(int UserId, out string CreatedAuthTockenId, out byte[] CreatedKey)
		{
			string TockenId = null!;
			do
			{
				TockenId = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator
					.GetBytes(this.TockenIdStringLength));
			} while (this.AuthTockens.ContainsKey(TockenId));

			var Key = System.Security.Cryptography.RandomNumberGenerator
				.GetBytes(this.TokenKeyLengthBytes);
			var ValidThrough = DateTime.UtcNow.AddSeconds(this.TockenLifeTimeSecs);

			CreatedAuthTockenId = TockenId;
			CreatedKey = Key;

			this.AuthTockens.Add(TockenId, new(Key, ValidThrough, UserId));
		}
		public bool ConfirmPasswordAndInvalidateTocken(
			string TockenId, byte[] PasswordHashedByClient, byte[] PasswordFromDb, out int? UserId)
		{
			UserId = null;
			if (!this.AuthTockens.TryGetValue(TockenId, out var GotTockenData))
			{
				return false;
			}
			UserId = GotTockenData.UserId;

			if (GotTockenData.ValidThrough < DateTime.UtcNow)
			{
				this.AuthTockens.Remove(TockenId);
				return false;
			}

			var MyTockenKey = GotTockenData.HashKey;

			var HashedPasswordFromDb = HashAlg(PasswordFromDb.Concat(MyTockenKey).ToArray());

			if (HashedPasswordFromDb.SequenceEqual(PasswordHashedByClient))
			{
				this.AuthTockens.Remove(TockenId);
				return true;
			}

			return false;
		}

		protected override async Task ExecuteAsync(System.Threading.CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				await Task.Delay(this.TockensCleanUpIntervalSecs * 1000);
				this.AuthTockens = this.AuthTockens.Where(s => s.Value.ValidThrough > DateTime.UtcNow)
					.ToDictionary(key => key.Key, value => value.Value);
			}
		}
	}
}
