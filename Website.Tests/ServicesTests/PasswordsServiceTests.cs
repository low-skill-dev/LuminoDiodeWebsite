using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Website.Services;
using Website.Services.SettingsProviders;
using Moq;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace WebsiteTests.ServicesTests
{
	public class PasswordsServiceTests
	{
		public PasswordsServiceTests()
		{

		}

		public string GenerateRandomPassword(int len)
		{
			// 3 bytes to 4 symb
			var pass = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes((int)Math.Ceiling(len / 4d * 3d)));

			if (pass.Length == len) return pass;
			if (pass.Length > len) return new string(pass.Take(len).ToArray());

			throw new AggregateException();
		}

		[Fact]
		public void CanConfirmPasswordAndGenerateSaltWithGivenLength()
		{
			Mock<PasswordsCryptographyServiceSettingsProvider> passSP = new(null);
			var rnd = new Random();
			for (int i = 64; i <= 512; i += rnd.Next(1, 33))
			{
				passSP.SetupGet(sp => sp.SaltSizeBytes).Returns(i);

				var serv = new PasswordsService(null, passSP.Object); // interactions with db is not being tested

				var origPass = GenerateRandomPassword(rnd.Next(3, 1000));

				var Hashed = serv.HashPassword(origPass, out var generatedSalt);

				Assert.True(Hashed.Length == (512 / 8)); // sha512 being used
				Assert.True(generatedSalt.Length == i);
				Assert.True(serv.ConfirmPassword(origPass, Hashed, generatedSalt));
			}
		}


		[Fact]
		public void CanDeclineWrongPassword()
		{
			/* 3 cases:
			 * 1. wrong pass, orig salt and hash
			 * 2. wrong salt, orig pass and hash
			 * 3. wrong hash, orig pass and salt
			 */

			var rnd = new Random();

			Mock<PasswordsCryptographyServiceSettingsProvider> passSP = new(null);
			passSP.SetupGet(sp => sp.SaltSizeBytes).Returns(128);
			var serv = new PasswordsService(null, passSP.Object);

			string origPass = GenerateRandomPassword(rnd.Next(3, 1000));
			string fakePass;
			do
				fakePass = GenerateRandomPassword(rnd.Next(3, 1000));
			while (origPass.Equals(fakePass));

			var origHash = serv.HashPassword(origPass, out var origSalt);
			serv.HashPassword(GenerateRandomPassword(rnd.Next(3, 1000)), out var fakeSalt);

			var fakeHash = serv.HashPassword(GenerateRandomPassword(rnd.Next(3, 1000)),out var dummy);

			// origPass, fakePass; origHash, fakeHash; origSalt, fakeSalt; - done

			// case 1
			Assert.False(serv.ConfirmPassword(fakePass, origHash, origSalt));
			// case 2
			Assert.False(serv.ConfirmPassword(origPass, origHash, fakeSalt));
			// case 3
			Assert.False(serv.ConfirmPassword(origPass, fakeHash, origSalt));
			// additional check 1 - all origs
			Assert.True(serv.ConfirmPassword(origPass, origHash, origSalt));
			// additional check 2 - given fakes are not connected
			Assert.False(serv.ConfirmPassword(fakePass, fakeHash, fakeSalt));
		}

	}
}
