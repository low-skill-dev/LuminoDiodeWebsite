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
	
	public class RequestsFromIpCounterServiceTests
	{
		private byte[] defaultIPBytes;

		public RequestsFromIpCounterServiceTests()
		{
			defaultIPBytes = new byte[] { 192, 168, 1, 2 };
		}

		[Fact]
		public void WillBan()
		{
			Mock<RequestsFromIpCounterServiceSettingsProvider> MockAntispamSP=new(null);
			MockAntispamSP.SetupGet(sp => sp.AllowedNumOfRequestsPerPeriod).Returns(2); // Allows 2 requests, ban on 3rd
			MockAntispamSP.SetupGet(sp => sp.ControlledPeriod_secs).Returns(10); // 2 req every 10 secs
			MockAntispamSP.SetupGet(sp => sp.UnbanInterval_secs).Returns(3); // check for unbans every 3 secs

			var serv = new RequestsFromIpCounterService(MockAntispamSP.Object); // no execution needed for this test

			serv.CountRequest(new System.Net.IPAddress(defaultIPBytes));
			serv.CountRequest(new System.Net.IPAddress(defaultIPBytes)); // 2nd

			Assert.False(serv.IPAddressIsBanned(new System.Net.IPAddress(defaultIPBytes))); // no ban expected

			serv.CountRequest(new System.Net.IPAddress(defaultIPBytes)); // 3rd req

			Assert.True(serv.IPAddressIsBanned(new System.Net.IPAddress(defaultIPBytes))); // ban expected
		}

		[Fact]
		public async Task WillUnban()
		{
			Mock<RequestsFromIpCounterServiceSettingsProvider> MockAntispamSP = new(null);
			MockAntispamSP.SetupGet(sp => sp.AllowedNumOfRequestsPerPeriod).Returns(2); // Allows 2 requests, ban on 3rd
			MockAntispamSP.SetupGet(sp => sp.ControlledPeriod_secs).Returns(3); // 2 req every 2 secs
			MockAntispamSP.SetupGet(sp => sp.UnbanInterval_secs).Returns(3); // check for unbans every 3 secs

			var serv = new RequestsFromIpCounterService(MockAntispamSP.Object);
			await serv.StartAsync(new System.Threading.CancellationToken(false)); // starting service execution

			for(int i =0; i<3;i++)	serv.CountRequest(new System.Net.IPAddress(this.defaultIPBytes)); // gets ban here

			Assert.True(serv.IPAddressIsBanned(new System.Net.IPAddress(this.defaultIPBytes)));

			await Task.Delay(3 * 1000 + 100); //wait 3 secs and 100 ms more for confidence;

			Assert.False(serv.IPAddressIsBanned(new System.Net.IPAddress(this.defaultIPBytes))); // should get unban already
		}
	}
}
