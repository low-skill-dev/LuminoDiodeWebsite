using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Services.SettingsProviders;
using Microsoft.IdentityModel.Tokens;

namespace Website.Services
{
	public class SessionInfo
	{
		public int UserId { get; init; }
		public DateTime ValidThrough { get; set; }

		public SessionInfo(int UserId, DateTime ValidThrough)
		{
			this.UserId = UserId;
			this.ValidThrough = ValidThrough;
		}

		public bool IsValidNow => this.ValidThrough > DateTime.UtcNow;
	}
	public class SessionManager : BackgroundService
	{
		public const string SessionIdCoockieName = "SessionId";

		/// <summary>
		/// Dictionary with SessiodId as key, (int UserId, DateTime ValidThrough) as Value.
		/// </summary>
		private Dictionary<string, SessionInfo> Sessions;
		private readonly SessionManagerServiceSettingsProvider SettingsProvider;
		private int SessionLifeTimeSecs
			=> this.SettingsProvider.SessionLifetime_secs;
		private int SessionIdLength
			=> this.SettingsProvider.SessionIdStringLength_bytes;
		private int SessionsCleanUpIntervalSecs
			=> this.SettingsProvider.SessionsCleanUpInterval_secs;

		public SessionInfo this[string SessionId] => this.Sessions[SessionId];

		public SessionManager(SessionManagerServiceSettingsProvider SettingsProvider)
		{
			this.SettingsProvider = SettingsProvider;
			this.Sessions = new();
		}

		public bool ValidateSession(string SessionId, out SessionInfo? FoundSession)
		{
			FoundSession = null;

			// if session does not exist
			if (!this.Sessions.TryGetValue(SessionId, out FoundSession)) return false;

			// if session is outdated
			if (!FoundSession.IsValidNow) return false;

			// if session is valid, validate it for the next 24 hours (by default)
			FoundSession.ValidThrough = DateTime.UtcNow.AddSeconds(this.SessionLifeTimeSecs);
			return true;
		}

		public void CreateSession(int UsedId, out string CreatedSessionId)
		{
			string SessionId = null!;
			do {
				SessionId = Base64UrlEncoder.Encode(System.Security.Cryptography.RandomNumberGenerator
					.GetBytes(this.SessionIdLength));
			} while(this.Sessions.ContainsKey(SessionId));
			var ValidThrough = DateTime.UtcNow.AddSeconds(this.SessionLifeTimeSecs);

			CreatedSessionId = SessionId;
			this.Sessions.Add(SessionId, new SessionInfo(UsedId, ValidThrough));
		}

		protected override async Task ExecuteAsync(System.Threading.CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				await Task.Delay(this.SessionsCleanUpIntervalSecs * 1000);
				this.Sessions = this.Sessions.Where(s => s.Value.IsValidNow).ToDictionary(key => key.Key, value => value.Value);
			}
		}
	}
}
