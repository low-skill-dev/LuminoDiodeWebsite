using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Website.Services;
using Website.Services.SettingsProviders;
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
using Microsoft.IdentityModel.Tokens;

namespace WebsiteTests.ServicesTests
{
	public class SessionManagerTests
	{
		private int UsedUserId;

		public SessionManagerTests()
		{
			this.UsedUserId = 100;
		}

		[Fact]
		public void CanCreateAndValidateSession()
		{
			Mock<SessionManagerServiceSettingsProvider> mockManagerSP = new(null);
			mockManagerSP.SetupGet(sp => sp.SessionLifetime_secs).Returns(1);
			mockManagerSP.SetupGet(sp => sp.SessionIdStringLength_bytes).Returns(512);

			SessionManager ssm = new(mockManagerSP.Object);

			ssm.CreateSession(UsedUserId, out var createdSSID);

			Assert.True(ssm.ValidateSession(createdSSID, out var foundSess));
			Assert.True(foundSess.UserId.Equals(UsedUserId));
		}

		[Fact]
		public async Task CanInvalidateSession()
		{
			Mock<SessionManagerServiceSettingsProvider> mockManagerSP = new(null);
			mockManagerSP.SetupGet(sp => sp.SessionLifetime_secs).Returns(1);
			mockManagerSP.SetupGet(sp => sp.SessionsCleanUpInterval_secs).Returns(1);
			mockManagerSP.SetupGet(sp => sp.SessionIdStringLength_bytes).Returns(512);

			SessionManager ssm = new(mockManagerSP.Object);
			await ssm.StartAsync(new System.Threading.CancellationToken(false));

			ssm.CreateSession(UsedUserId, out var createdSSID);

			Assert.True(ssm.ValidateSession(createdSSID, out var foundSess));
			Assert.True(foundSess.UserId.Equals(UsedUserId));
			Assert.True(foundSess.IsValidNow);

			await Task.Delay(2000); // wait 1 sec, should invalidate

			Assert.False(ssm.ValidateSession(createdSSID, out foundSess));
			Assert.True(foundSess is null);
		}

		[Fact]
		public void CanCreateIdWithGivenLength()
		{
			int sessIdLength = new Random().Next(128,512+1);

			Mock<SessionManagerServiceSettingsProvider> mockManagerSP = new(null);
			mockManagerSP.SetupGet(sp => sp.SessionLifetime_secs).Returns(1);
			mockManagerSP.SetupGet(sp => sp.SessionIdStringLength_bytes).Returns(sessIdLength);

			SessionManager ssm = new(mockManagerSP.Object);
			ssm.CreateSession(UsedUserId, out var createdSSID);

			Assert.True(Base64UrlEncoder.DecodeBytes(createdSSID).Length.Equals(sessIdLength));
		}

		[Fact]
		public void CanCreateSessionWithGivenLifetime()
		{
			Random rnd = new();
			var usedTimeSpan = new TimeSpan(rnd.Next(0,24),rnd.Next(0,60),rnd.Next(5,60));

			Mock<SessionManagerServiceSettingsProvider> mockManagerSP = new(null);
			mockManagerSP.SetupGet(sp => sp.SessionLifetime_secs).Returns((int)usedTimeSpan.TotalSeconds);
			mockManagerSP.SetupGet(sp => sp.SessionIdStringLength_bytes).Returns(512);

			SessionManager ssm = new(mockManagerSP.Object);
			ssm.CreateSession(UsedUserId, out var createdSSID);
			ssm.ValidateSession(createdSSID, out var sess);

			var sessValidThr = sess.ValidThrough;
			var OneSecondLess = DateTime.UtcNow.Add(usedTimeSpan).AddSeconds(-1);
			var OneSecondMore = DateTime.UtcNow.Add(usedTimeSpan).AddSeconds(+1);

			Assert.True(OneSecondLess < sessValidThr && sessValidThr < OneSecondMore);
		}
	}
}
