using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.DocumentModel;
using Website.Repository;

namespace Website.Services
{
	/// <summary>
	/// Scope service which processes user document search requests and passes this request to
	/// FrequentRequestsSingletonService (tbd)
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

		public virtual DateTime ProceedDateTime { get;private set; }
		public virtual string? Request { get; private set; }
		public virtual List<DbDocument>? Response { get; private set; }


		// ДОБАВИТЬ ОБНОВЛЕНИЕ ОТВЕТОВ ДЛЯ ЗАПРОСОВ КОТРЫЕ ОСТАЮТСЯ ЧАСТЫМИ ДОЛГОЕ ВРЕМЯ
		public async Task<List<DbDocument>> ProceedRequest(string UserRequest)
		{
			this.ProceedDateTime = DateTime.UtcNow;

			var TryGetFromFreq = await this.FreqReqService.GetSimilarRequestOrNull(UserRequest);
			if (TryGetFromFreq is not null && TryGetFromFreq.Response is not null)
			{
				return TryGetFromFreq.Response;
			}

			this.Request = UserRequest;
			this.Response = await this.DbContextScopeFactory.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>().DbDocuments
				.OrderByDescending(d => d.TitleTsVector.Rank(EF.Functions.WebSearchToTsQuery(UserRequest))).Take(20).Include("Author")
				.ToListAsync();

			this.FreqReqService.AddDocumentSearchServiceScope(this);

			return this.Response;
		}
	}
}
