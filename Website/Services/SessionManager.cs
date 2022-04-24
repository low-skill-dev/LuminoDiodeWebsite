using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Website.Services.SettingsProviders;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Website.Services
{
	public class SessionInfo
	{
		public int UserId { get; set; }
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
		private readonly AppSettingsProvider SettingsProvider;
		private int SessionLifeTimeSecs
			=> this.SettingsProvider.SessionManagerServiceSP.SessionLifetime_secs;
		private int SessionIdStringLength
			=> this.SettingsProvider.SessionManagerServiceSP.SessionIdStringLength_chars;
		private int SessionsCleanUpIntervalSecs
			=> this.SettingsProvider.SessionManagerServiceSP.SessionsCleanUpInterval_secs;

		public SessionInfo this[string SessionId] => this.Sessions[SessionId];

		public SessionManager(AppSettingsProvider SettingsProvider)
		{
			this.SettingsProvider = SettingsProvider;
			this.Sessions = new();
		}

		public bool ValidateSession(string SessionId, out SessionInfo? FoundSession)
		{
			FoundSession=null;

			// if session does not exist
			if (!this.Sessions.TryGetValue(SessionId, out FoundSession)) return false;

			// if session is outdated
			if ((FoundSession.ValidThrough.Ticks - DateTime.UtcNow.Ticks) < 0) return false;

			// if session is valid, validate it for the next 24 hours (by default)
			FoundSession.ValidThrough = DateTime.UtcNow.AddSeconds(SessionLifeTimeSecs);
			return true;
		}

		public void CreateSession(int UsedId, out string CreatedSessionId)
		{
			var SessionId = new string(System.Security.Cryptography.RandomNumberGenerator.GetBytes(SessionIdStringLength).Select(x => (char)x).ToArray());
			var ValidThrough = DateTime.UtcNow.AddSeconds(this.SessionLifeTimeSecs);

			CreatedSessionId = SessionId;
			this.Sessions.Add(SessionId, new SessionInfo(UsedId, ValidThrough));
		}

		protected override async Task ExecuteAsync(System.Threading.CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				await Task.Delay(SessionsCleanUpIntervalSecs * 1000);
				this.Sessions = this.Sessions.Where(s => s.Value.IsValidNow).ToDictionary(key => key.Key, value => value.Value);
			}
		}
	}
}
