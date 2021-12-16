using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Repository;

namespace Website.Services
{
	/// <summary>
	/// Singleton service which provides <B>five</B> recent posted documents by making requsts to db by the interval.
	/// </summary>
	public class RecentDocumentsBackgroundService : BackgroundService
	{
		private readonly IServiceScopeFactory DbContextScopeFactory;
		public int Interval_msec = 1000 * 60 * 5; // 5 min interval

		public RecentDocumentsBackgroundService(IServiceScopeFactory DbContextScopeFactory)
		{
			this.DbContextScopeFactory = DbContextScopeFactory;
		}

		public List<Website.Models.DocumentModel.DbDocument> RecentDocuments { get; private set; }

		protected async override Task ExecuteAsync(CancellationToken ct)
		{
			using (WebsiteContext context = this.DbContextScopeFactory
				.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>())
			{
				while (!ct.IsCancellationRequested)
				{
					var NewRecent = context.DbDocuments.OrderByDescending(d => d.CreatedDateTime).Take(5).Include("Author");
					this.RecentDocuments = NewRecent.ToList();
					await Task.Delay(this.Interval_msec);
				}
			}
		}
	}
}
