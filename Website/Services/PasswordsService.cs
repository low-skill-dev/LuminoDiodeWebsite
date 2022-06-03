using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Cryptography;
using Website.Services.SettingsProviders;
using System.Text;

namespace Website.Services
{
	public class PasswordsService
	{
		private readonly IServiceScopeFactory DbContextScopeFactory;
		private readonly PasswordsCryptographyServiceSettingsProvider SettingsProvider;
		private readonly Func<byte[], byte[]> HashAlg = SHA512.HashData;

		private int SaltSizeBytes => this.SettingsProvider.SaltSizeBytes;

		public PasswordsService(IServiceScopeFactory DbContextScopeFactory, PasswordsCryptographyServiceSettingsProvider SettingsProvider)
		{
			this.DbContextScopeFactory = DbContextScopeFactory;
			this.SettingsProvider = SettingsProvider;
		}

		private byte[] GenerateSalt()
			=> System.Security.Cryptography.RandomNumberGenerator.GetBytes(SaltSizeBytes);
		public byte[] HashPassword(string PlainTextPassword, out byte[] GeneratedSalt)
		{
			var Salt = GenerateSalt();
			var PasswordBytes = Encoding.UTF8.GetBytes(PlainTextPassword);

			var SaltedPassword = PasswordBytes.Concat(Salt).ToArray();

			GeneratedSalt = Salt;
			return this.HashAlg(SaltedPassword); // should better use PBKDF2 ?
		}
		public bool ConfirmPassword(string PlainTextPassword, byte[] HashedPassword, byte[] Salt)
		{
			var PasswordBytes = Encoding.UTF8.GetBytes(PlainTextPassword);

			var SaltedPossiblePassword = PasswordBytes.Concat(Salt).ToArray();

			return this.HashAlg(SaltedPossiblePassword).SequenceEqual(HashedPassword);
		}
		public async void SetPassAndSaltForUser(int SelectedUserId, string PasswordPlaintText)
		{
			var ctx = this.DbContextScopeFactory.CreateScope().ServiceProvider
				.GetRequiredService<Website.Repository.WebsiteContext>();
			var User = (await ctx.Users.FindAsync(SelectedUserId));
			var Hashed = this.HashPassword(PasswordPlaintText, out var Salt);
			User.AuthHashedPassword = Hashed;
			User.AuthPasswordSalt = Salt;
			ctx.Update(User);
			ctx.SaveChanges();
		}
	}
}