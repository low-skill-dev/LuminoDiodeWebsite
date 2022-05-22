using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;

namespace Website.Controllers
{
	public sealed class DbgController : AMyController
	{
		private readonly Website.Services.FrequentSearchRequestsService freqServ;
		public DbgController(IServiceScopeFactory ScopeFactory) : base(ScopeFactory)
		{
			var sp = ScopeFactory.CreateScope().ServiceProvider;
			this.freqServ = sp.GetRequiredService<FrequentSearchRequestsService>();
		}

		public ViewResult Summary()
		{
			return this.View(model: (this.context.DbDocuments.AsQueryable()));
		}
		/*
		public ViewResult Show1()
		{
			return this.View(this.freqServ.FrequentRequests.Select(x => x.DocumentSearchServiceScope.Request + '\t' + x.Frequency.ToString()));
		}
		*/
	}
}
