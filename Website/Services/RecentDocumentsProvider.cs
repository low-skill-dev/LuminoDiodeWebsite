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
	public class RecentDocumentsProvider:BackgroundService
	{
		private WebsiteContext context;
		private int Interval_msec = 1000 * 60 * 1; // 1 min interval

		public IQueryable<Website.Models.DocumentModel.DbDocument> RecentDocuments { get; private set; }

		protected async override Task ExecuteAsync(CancellationToken ct)
		{
			while (ct.IsCancellationRequested)
			{
				var NewRecent = context.DbDocuments.OrderBy(d => d.CreatedDateTime);
				NewRecent.Skip(NewRecent.Count() - 5);
				this.RecentDocuments = NewRecent;
				await Task.Delay(this.Interval_msec);
			}
		}
	}
}
