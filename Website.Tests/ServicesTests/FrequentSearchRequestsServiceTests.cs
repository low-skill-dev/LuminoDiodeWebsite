using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Xunit.Abstractions;
using RandomDataGenerator;
using Website.Models.DocumentModel;

namespace WebsiteTests.ServicesTests
{
	public class FrequentSearchRequestsServiceTests
	{
		private Random rnd;

		public FrequentSearchRequestsServiceTests()
		{
			rnd = new Random();
		}

		private Mock<DocumentSearchService> GenerateDocumentSearchScope()
		{
			Mock<DocumentSearchService> scp = new Mock<DocumentSearchService>(null,null);
			var doc = Document.GenerateRandom();

			scp.SetupGet(scope => scope.Request).Returns(doc.Title);
			scp.SetupGet(scope => scope.Response).Returns(new List<DbDocument>(new DbDocument[] { DbDocument.FromDocument(doc) }));
			scp.SetupGet(scope => scope.ProceedDateTime).Returns(DateTime.UtcNow);

			return scp;
		}

		[Fact]
		public async void CanCallUpdateByQuantityIntervalAndThenFind()
		{
			Mock<FrequentSearchRequestsServiceSettingsProvider> freqSP = new(null);

			var quantityToUpdate = 20;

			freqSP.SetupGet(sp => sp.Interval_msec).Returns(1000); // period of ExecuteAsync
			freqSP.SetupGet(sp => sp.Interval_numOfRecentRequests).Returns(quantityToUpdate); // max num of recent
			freqSP.SetupGet(sp=> sp.NumOfFrequentRequestsStored).Returns(50); // max num of freq
			freqSP.SetupGet(sp => sp.TokenSortRationNeededToCountAsSimilar).Returns(50); // 
			freqSP.SetupGet(sp => sp.Interval_updateNeededCheck_msec).Returns(10000); // period of cheking if response lifetime expired
			freqSP.SetupGet(sp => sp.ResponseLifetime_msec).Returns(int.MaxValue); // max val - prevent interactions with db

			var serv = new FrequentSearchRequestsService(null, freqSP.Object);
			var docs = new object[quantityToUpdate + 1].Select(x => GenerateDocumentSearchScope().Object).ToList();

			for(int i =0; i< docs.Count-1;i++) // we dont know will it update on 20th or on 20+1
				Assert.Null(await serv.GetSimilarRequestOrNull(docs[i].Request));

			docs.ForEach(x => serv.AddDocumentSearchServiceScope(x));
			await serv.StartAsync(new System.Threading.CancellationToken(false)); // starting service

			for (int i = 0; i < docs.Count - 1; i++)
				Assert.NotNull(await serv.GetSimilarRequestOrNull(docs[i].Request));
		}

		[Fact]
		public async void CanCallUpdateByTimeIntervalAndThenFind()
		{
			Mock<FrequentSearchRequestsServiceSettingsProvider> freqSP = new(null);

			var timeToUpdate = 3000;
			var updateCheckInterval = 100;

			freqSP.SetupGet(sp => sp.Interval_msec).Returns(timeToUpdate); // period of ExecuteAsync
			freqSP.SetupGet(sp => sp.Interval_numOfRecentRequests).Returns(int.MaxValue); // max num of recent
			freqSP.SetupGet(sp => sp.NumOfFrequentRequestsStored).Returns(50); // max num of freq
			freqSP.SetupGet(sp => sp.TokenSortRationNeededToCountAsSimilar).Returns(50); // 
			freqSP.SetupGet(sp => sp.Interval_updateNeededCheck_msec).Returns(updateCheckInterval); // period of cheking if response lifetime expired
			freqSP.SetupGet(sp => sp.ResponseLifetime_msec).Returns(int.MaxValue); // max val - prevent interactions with db

			var serv = new FrequentSearchRequestsService(null, freqSP.Object);
			await serv.StartAsync(new System.Threading.CancellationToken(false)); // starting service

			var docs = new object[rnd.Next(3,20)].Select(x => GenerateDocumentSearchScope().Object).ToList();

			for (int i = 0; i < docs.Count - 1; i++) // we dont know will it update on 20th or on 20+1
				Assert.Null(await serv.GetSimilarRequestOrNull(docs[i].Request));

			docs.ForEach(x => serv.AddDocumentSearchServiceScope(x));
			await Task.Delay(timeToUpdate+ updateCheckInterval); // waiting to update by time interval

			for (int i = 0; i < docs.Count - 1; i++)
				Assert.NotNull(await serv.GetSimilarRequestOrNull(docs[i].Request));
		}
	}
}
