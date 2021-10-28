using Microsoft.EntityFrameworkCore;
using Website.Repository.Models;

namespace Website.Repository
{
	public class WebsiteContext:DbContext
	{
		public WebsiteContext(DbContextOptions<WebsiteContext> options) : base(options)
		{

		}

		public DbSet<DbArticle> DbArticles;
	}
}
