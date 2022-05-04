//#if DEBUG

//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Website.Services.SettingsProviders;
//using Microsoft.Extensions.DependencyInjection;
//using System.Security.Cryptography;
//using System.Text;

//namespace Website.Services
//{
//	[Obsolete]
//	public class AuthTockenService : BackgroundService
//	{
//		public const string TockenIdCoockieName = "AuthTocken";
//		public const string HashAlgUsed = "SHA512";
//		public Func<byte[], byte[]> HashAlg = SHA512.HashData;

//		public Dictionary<string, (string HashKeyString64, DateTime ValidThrough, int UserId)> AuthTockens { get; private set; } = new();

//		private readonly AuthTockenServiceSettingsProvider SettingsProvider;
//		private int TockenLifeTimeSecs
//			=> this.SettingsProvider.TockenLifetime_secs;
//		private int TockenIdStringLength
//			=> this.SettingsProvider.TockenIdStringLength_chars;
//		private int TokenKeyLengthBytes
//			=> this.SettingsProvider.TokenKeyLength_bytes;
//		private int TockensCleanUpIntervalSecs
//			=> this.SettingsProvider.TokensCleanUpInterval_secs;

//		public AuthTockenService(AppSettingsProvider SettingsProvider)
//		{
//			this.SettingsProvider = SettingsProvider.AuthTockenServiceSP;
//		}

//		public void CreateTocken(int UserId, out string CreatedAuthTockenId, out string CreatedKeyString64)
//		{
//			string TockenId = null!;
//			do
//			{
//				TockenId = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator
//					.GetBytes(this.TockenIdStringLength));
//			} while (this.AuthTockens.ContainsKey(TockenId));

//			var Key = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator
//				.GetBytes(this.TokenKeyLengthBytes));
//			var ValidThrough = DateTime.UtcNow.AddSeconds(this.TockenLifeTimeSecs);

//			CreatedAuthTockenId = TockenId;
//			CreatedKeyString64 = Key;

//			this.AuthTockens.Add(TockenId, new(Key, ValidThrough, UserId));
//		}

//		public bool ConfirmPassword
//			(string TockenId, string PasswordHashedByClientString64,
//			string PasswordFromDbString64, out int? UserId)
//		{
//			return this.ConfirmPasswordPrivate(TockenId, true, PasswordHashedByClientString64, PasswordFromDbString64, out UserId);
//		}
//		public bool ConfirmPasswordWithoutTockenInvalidation(
//			string TockenId, string PasswordHashedByClientString64,
//			string PasswordFromDbString64, out int? UserId)
//		{
//			return this.ConfirmPasswordPrivate(TockenId, false, PasswordHashedByClientString64, PasswordFromDbString64, out UserId);
//		}
//		public void InvalidateTocken(string TockenId)
//		{
//			if (this.AuthTockens.ContainsKey(TockenId))
//				this.AuthTockens.Remove(TockenId);
//		}

//		private bool ConfirmPasswordPrivate(
//			string TockenId,
//			bool InvalidateTocken,
//			string PasswordHashedByClientString64,
//			string PasswordFromDbString64, out int? UserId)
//		{
//			UserId = null;
//			if (!this.AuthTockens.TryGetValue(TockenId, out var GotTockenData))
//			{
//				return false;
//			}
//			UserId = GotTockenData.UserId;

//			if (GotTockenData.ValidThrough > DateTime.UtcNow)
//			{
//				this.AuthTockens.Remove(TockenId);
//				return false;
//			}

//			var MyTockenKey = GotTockenData.HashKeyString64;

//			var HashedPasswordFromDb = SHA512.HashData(Encoding.UTF8.GetBytes(PasswordFromDbString64 + MyTockenKey));
//			if (HashedPasswordFromDb.SequenceEqual(Encoding.UTF8.GetBytes(PasswordHashedByClientString64)))
//			{
//				if (InvalidateTocken) this.AuthTockens.Remove(TockenId);
//				return true;
//			}

//			return false;
//		}

//		protected override async Task ExecuteAsync(System.Threading.CancellationToken ct)
//		{
//			while (!ct.IsCancellationRequested)
//			{
//				await Task.Delay(this.TockensCleanUpIntervalSecs * 1000);
//				this.AuthTockens = this.AuthTockens.Where(s => s.Value.ValidThrough > DateTime.UtcNow)
//					.ToDictionary(key => key.Key, value => value.Value);
//			}
//		}
//	}
//}

//#endif