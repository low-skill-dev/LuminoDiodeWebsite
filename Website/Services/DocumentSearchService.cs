using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Website.Models.DocumentModel;
using Website.Repository;

namespace Website.Services
{
	/// <summary>
	/// A scope service which processes user document search requests and passes this request to
	/// FrequentRequestsService (Singleton)
	/// </summary>
	public class DocumentSearchService
	{
		private readonly IServiceScopeFactory DbContextScopeFactory;
		private readonly FrequentSearchRequestsService FreqReqService;
		public DocumentSearchService(IServiceScopeFactory DbContextScopeFactory, FrequentSearchRequestsService FreqReqService)
		{
			this.DbContextScopeFactory = DbContextScopeFactory;
			this.FreqReqService = FreqReqService;
		}

		public DateTime ProceedDateTime;

		public string Request { get; private set; }
		public List<DbDocument> Response { get; private set; }

		public List<DbDocument> ProceedRequest(string UserRequest)
		{
			this.ProceedDateTime = DateTime.UtcNow;

			var TryGetFromFreq = this.FreqReqService.GetSimilarRequestOrNull(UserRequest);
			if (TryGetFromFreq != null)
				return TryGetFromFreq.Response;


			this.Request = UserRequest;
			this.Response = this.DbContextScopeFactory.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>().DbDocuments
				.OrderByDescending(d => d.TitleTsVector.Rank(EF.Functions.WebSearchToTsQuery(UserRequest))).Take(20).Include("Author")
				.ToList();

			this.FreqReqService.AddDocumentSearchServiceScope(this);

			return this.Response;
		}
	}
}
