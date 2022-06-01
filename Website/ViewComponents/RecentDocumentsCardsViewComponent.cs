using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Website.Models.ViewModels;
using Website.Services;
using System.Linq;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace Website.ViewComponents
{
	public class RecentDocumentsCardsViewComponent:ViewComponent
	{
		public const string nameofThis = "RecentDocumentsCards";

		private readonly Website.Repository.WebsiteContext context;
		private readonly RecentDocumentsBackgroundService recentDocuments;

		public RecentDocumentsCardsViewComponent(Website.Repository.WebsiteContext context, RecentDocumentsBackgroundService recentDocuments)
		{
			this.context=context;
			this.recentDocuments = recentDocuments;
		}

		public async Task<IViewComponentResult> InvokeAsync(int? filterByUserId = null, int? MaxQuantity = null) 
		{
			IEnumerable<Website.Models.DocumentModel.Document> result;

			if (filterByUserId.HasValue)
				result= await this.context.DbDocuments.Include("Author").Where(x => x.Author!.Id == filterByUserId.Value).OrderByDescending(x => x.CreatedDateTime).Select(x => x.ToDocument()).ToListAsync();
			else
				result = recentDocuments.RecentDocuments;

			if (MaxQuantity.HasValue)
				result = result.Take(MaxQuantity.Value);

			return View(result);
		}
	}
}
