using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using Website.Models.DocumentModel;

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

		public string Request { get; private set; }
		public IQueryable<DbDocument> Response { get; private set; }
		
		public IQueryable<DbDocument> ProceedRequest(string UserRequest)
		{
			var UserReqWords = new Regex(@"\w+").Matches(UserRequest).Select(x => x.Value).ToArray();
			var DocsIQ = this.DbContextScopeFactory.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>().DbDocuments;

			this.Request = UserRequest;
			this.Response = DocsIQ.OrderByDescending(d => EF.Functions.TrigramsWordSimilarity(d.Title,UserRequest));
			
			this.FreqReqService.AddDocumentSearchServiceScope(this);

			return this.Response;
		}
	}
}
