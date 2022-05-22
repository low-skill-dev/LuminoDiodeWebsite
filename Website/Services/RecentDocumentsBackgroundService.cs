using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Repository;
using Website.Services.SettingsProviders;
using Website.Services.SettingsProviders;

namespace Website.Services
{
	/// <summary>
	/// Singleton service which provides <B>five</B> recent posted documents by making requsts to db by the interval.
	/// </summary>
	public class RecentDocumentsBackgroundService : BackgroundService
	{
		private readonly WebsiteContext context;
		private readonly RecentDocumentsBackgroundServiceSettingsProvider SettingsProvider;
		private int Interval_msec => this.SettingsProvider.Interval_msec;

		public RecentDocumentsBackgroundService(IServiceScopeFactory DbContextScopeFactory, AppSettingsProvider SettingsProvider)
		{
			this.context = DbContextScopeFactory.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>();
			this.SettingsProvider = SettingsProvider.RecentDocumentsBackgroundServiceSP;
		}

		public List<Website.Models.DocumentModel.Document> RecentDocuments { get; private set; } = new List<Models.DocumentModel.Document>();

		protected async override Task ExecuteAsync(CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				var NewRecent = this.context.DbDocuments.OrderByDescending(x=> x.CreatedDateTime).Take(5).Include("Author"); // newest as first
				this.RecentDocuments = NewRecent.Select(x => x.ToDocument()).ToList();
				await Task.Delay(this.Interval_msec);
			}
		}
	}
}
