using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Repository;
using Website.Services.SettingsProviders;

namespace Website.Services
{
	/// <summary>
	/// Singleton service which provides <B>five</B> recent posted documents by making requsts to db by the interval.
	/// </summary>
	public class RecentDocumentsBackgroundService : BackgroundService
	{
		private readonly IServiceScopeFactory DbContextScopeFactory;
		private readonly AppSettingsProvider SettingsProvider;
		public int Interval_msec
		{
			get => this.SettingsProvider.RecentDocumentsBackgroundServiceSP.Interval_msec;
		}

		public RecentDocumentsBackgroundService(IServiceScopeFactory DbContextScopeFactory, AppSettingsProvider SettingsProvider)
		{
			this.DbContextScopeFactory = DbContextScopeFactory;
			this.SettingsProvider = SettingsProvider;
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
