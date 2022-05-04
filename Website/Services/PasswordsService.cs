using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Website.Services.SettingsProviders;

namespace Website.Services
{
	public class PasswordsService
	{
		private readonly IServiceScopeFactory DbContextScopeFactory;
		private readonly AppSettingsProvider SettingsProvider;
		public PasswordsService(IServiceScopeFactory DbContextScopeFactory, AppSettingsProvider SettingsProvider)
		{
			this.DbContextScopeFactory = DbContextScopeFactory;
			this.SettingsProvider = SettingsProvider;
		}

		private const int KeySizeBytes = 64; // 512 bits
		private readonly Func<byte[], byte[]> HashAlg = SHA512.HashData;
		private static string GenerateSalt()
		{
			var Bits = new byte[KeySizeBytes];

			System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(Bits);
			return Convert.ToBase64String(Bits);
		}
		public string HashPassword(string PlainTextPassword, out string GeneratedSalt)
		{
			var Salt = GenerateSalt();

			var SaltedPassword = PlainTextPassword + Salt;

			GeneratedSalt = Salt;
			return Convert.ToBase64String(this.HashAlg(Encoding.UTF8.GetBytes(SaltedPassword))); // should better use PBKDF2 ?
		}
		public bool ConfirmPassword(string PlainTextPassword, string HashedPassword, string Salt)
		{
			var SaltedPossiblePassword = PlainTextPassword + Salt;

			return this.HashAlg(Encoding.UTF8.GetBytes(SaltedPossiblePassword)).SequenceEqual(Convert.FromBase64String(HashedPassword));
		}
		public async void SetPassAndSaltForUser(int SelectedUserId, string PasswordPlaintText)
		{
			var ctx = this.DbContextScopeFactory.CreateScope().ServiceProvider
				.GetRequiredService<Website.Repository.WebsiteContext>();
			var User = (await ctx.Users.FindAsync(SelectedUserId));
			var Hashed = this.HashPassword(PasswordPlaintText, out var Salt);
			User.AuthHashedPasswordString64 = Hashed;
			User.AuthPasswordSaltString64 = Salt;
			ctx.Update(User);
			ctx.SaveChanges();
		}
	}
}
