using System.Security.Cryptography;
using FuzzySharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
		private Func<byte[], byte[]> HashAlg = SHA512.HashData;
		private static byte[] GenerateSalt()
		{
			var Bits = new byte[KeySizeBytes];
			System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(Bits);
			return Bits;
		}
		public byte[] HashPassword(string PlainTextPassword, out byte[] GeneratedSalt)
		{
			var Salt = GenerateSalt();

			var SaltedPassword = new byte[PlainTextPassword.Length + Salt.Length];
			for (int i = 0; i < PlainTextPassword.Length; i++) { SaltedPassword[i] = (byte)(PlainTextPassword[i]); }
			for (int i = 0; i < Salt.Length; i++) { SaltedPassword[PlainTextPassword.Length + i] = Salt[i]; }

			GeneratedSalt = Salt;
			return HashAlg(SaltedPassword); // should better use PBKDF2 ?
		}
		public bool ConfirmPassword(string PlainTextPassword, byte[] HashedPassword, byte[] Salt)
		{
			var SaltedPossiblePassword = new byte[PlainTextPassword.Length + Salt.Length];
			for (int i = 0; i < PlainTextPassword.Length; i++) { SaltedPossiblePassword[i] = (byte)(PlainTextPassword[i]); }
			for (int i = 0; i < Salt.Length; i++) { SaltedPossiblePassword[PlainTextPassword.Length + i] = Salt[i]; }

			return HashAlg(SaltedPossiblePassword).SequenceEqual(HashedPassword);
		}
		public async void SetPassAndSaltForUser(int SelectedUserId, string PasswordPlaintText)
		{
			var ctx = this.DbContextScopeFactory.CreateScope().ServiceProvider
				.GetRequiredService<Website.Repository.WebsiteContext>();
			var User = (await ctx.Users.FindAsync(SelectedUserId));
			//var Salt = GenerateSalt();
			var Hashed = HashPassword(PasswordPlaintText, out var Salt);
			User.AuthHashedPassword= Hashed;
			User.AuthPasswordSalt= Salt;
			ctx.Update(User);
			ctx.SaveChanges();
		}
	}
}
