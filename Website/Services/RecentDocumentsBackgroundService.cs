﻿using Microsoft.EntityFrameworkCore;
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

		private int _NumberOfStoredDocs = 5;
		public int NumberOfStoredDocs
		{
			get
			{
				return this._NumberOfStoredDocs;
			}
			set
			{
				if (0 < value && value < 1000) this._NumberOfStoredDocs = value;
				else throw new System.ArgumentOutOfRangeException();
			}
		}
		public RecentDocumentsBackgroundService(IServiceScopeFactory DbContextScopeFactory, RecentDocumentsBackgroundServiceSettingsProvider SettingsProvider)
		{
			this.context = DbContextScopeFactory.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>();
			this.SettingsProvider = SettingsProvider;
		}

		public List<Website.Models.DocumentModel.Document> RecentDocuments { get; private set; } = new List<Models.DocumentModel.Document>();

		protected async override Task ExecuteAsync(CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				var NewRecent = this.context.DbDocuments.OrderByDescending(x=> x.CreatedDateTime).Take(this.NumberOfStoredDocs).Include("Author"); // newest as first
				this.RecentDocuments = NewRecent.Select(x => x.ToDocument()).ToList();
				await Task.Delay(this.Interval_msec);
			}
		}
	}
}
