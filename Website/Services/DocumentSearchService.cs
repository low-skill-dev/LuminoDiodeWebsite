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
using FuzzySharp;

namespace Website.Services
{
	/// <summary>
	/// Scope service which processes user document search requests and passes this request to
	/// FrequentRequestsSingletonService (tbd)
	/// </summary>
	public class DocumentSearchService
	{
		private IServiceScopeFactory DbContextScopeFactory;
		public DocumentSearchService(IServiceScopeFactory DbContextScopeFactory/*,FrequentRequestsSingletonService*/)
		{
			this.DbContextScopeFactory = DbContextScopeFactory;
		}
		
		public async Task<IQueryable<DbDocument>> ProceedRequest(string UserRequest)
		{

			var DocsIQ = this.DbContextScopeFactory.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>().DbDocuments;
			return await Task<IQueryable<DbDocument>>.Run(() =>
			{
				// В некоторых строках может быть очень много похожих слов, поэтому сначала следует
				// применить сортировку по токенам, а потом уже непосредственно сравнить названия.
				// К тому же изначальную сортировку по всему запросу убивает порядок слов.
				return DocsIQ.OrderBy(d => Fuzz.TokenSetRatio(d.Title, UserRequest)).ThenBy(d=> Fuzz.Ratio(d.Title,UserRequest));
			});
		}
	}
}
