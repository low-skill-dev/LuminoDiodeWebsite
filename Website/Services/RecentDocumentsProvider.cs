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

namespace Website.Services
{
	//public interface IRecentDocumentsBackgroundService
	//{
	//	public IQueryable<Website.Models.DocumentModel.DbDocument> RecentDocuments { get; private set; }

	//	protected async override Task ExecuteAsync(CancellationToken ct);
	//}


	/// <summary>
	/// Provides recent posted document by making requsts to db by the interval.
	/// </summary>
	public class RecentDocumentsBackgroundService : BackgroundService
	{
		private IServiceScopeFactory DbContextScopeFactory;
		private int Interval_msec = 1000 * 60 * 1; // 1 min interval

		public RecentDocumentsBackgroundService(IServiceScopeFactory DbContextScopeFactory)
		{
			this.DbContextScopeFactory = DbContextScopeFactory;
		}


		public IQueryable<Website.Models.DocumentModel.DbDocument> RecentDocuments { get; private set; }

		protected async override Task ExecuteAsync(CancellationToken ct)
		{
			using (WebsiteContext context = this.DbContextScopeFactory
				.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>())
			{
				while (!ct.IsCancellationRequested)
				{
					var NewRecent = context.DbDocuments.OrderBy(d => d.CreatedDateTime);
					NewRecent.Skip(NewRecent.Count() - 5);
					this.RecentDocuments = NewRecent;
					await Task.Delay(this.Interval_msec);
				}
			}
		}
	}
}
