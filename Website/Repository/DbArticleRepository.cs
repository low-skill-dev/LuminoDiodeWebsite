//using System.Linq;
//using System.Collections.Generic;
//using Website.Repository.Models;
//using System;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.AspNetCore.Mvc;
//using Website.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Website.Repository;
//using Microsoft.EntityFrameworkCore;

//namespace Website.Repository
//{
//	public interface IDbArticleRepository
//	{
//		IQueryable<DbArticle> GetAllDbArticles();
//		public Task<DbArticle> GetDbArticleById(int id);
//		public IQueryable<DbArticle> GetDbArticlesByTitle(string title);
//		public IQueryable<DbArticle> GetDbArticlesByAuthorUserId(int authorUserId);
//		public IQueryable<DbArticle> GetDbArticlesOrderedByTagsMatch(string[] SearchTags);

//		public void InsertDbArticle(DbArticle dbArticle);
//		public void DeleteDbArticle(int DbArticleId);
//		public void UpdateDbArticle(DbArticle dbArticle);
//		public void Save();
//	}
//	public class DbArticleRepository : IDbArticleRepository
//	{
//		private WebsiteContext context;
//		public DbArticleRepository(WebsiteContext context)
//		{
//			this.context = context;
//		}
//		public static DbArticleRepository CreateUsingNpgSql(string connectionString =
//			"Server=localhost;Database=LuminodiodeWebsiteDb;Password=qwerty;username=postgres")
//		{
//			ServiceCollection sc = new ServiceCollection();
//			sc.AddDbContext<WebsiteContext>(opts => { opts.UseNpgsql(connectionString); });
//			sc.BuildServiceProvider();
//			return new DbArticleRepository(sc.BuildServiceProvider().GetRequiredService<WebsiteContext>());
//		}

//		public IQueryable<DbArticle> GetAllDbArticles()
//		{
//			return this.context.DbArticles.AsQueryable();
//		}
//		public async Task<DbArticle> GetDbArticleById(int id)
//		{
//			return await this.context.DbArticles.FindAsync(id);
//		}
//		public IQueryable<DbArticle> GetDbArticlesByTitle(string title)
//		{
//			return this.context.DbArticles.Where(x => x.Title == title);
//		}
//		public IQueryable<DbArticle> GetDbArticlesByAuthorUserId(int authorUserId)
//		{
//			return this.context.DbArticles.Where(x => x.AuthorUserId == authorUserId);
//		}
//		/// <summary>
//		/// <b>Be careful!</b> 
//		/// This Queryable might be very slow.
//		/// </summary>
//		public IQueryable<DbArticle> GetDbArticlesOrderedByTagsMatch(string[] SearchTags)
//		{
//			// По совпадению максимального числа тегов
//			return this.context.DbArticles.OrderBy(x =>
//				x.Tags.Length - x.Tags.Except(SearchTags).Count());
//		}
//		public async void InsertDbArticle(DbArticle dbArticle)
//		{
//			await this.context.DbArticles.AddAsync(dbArticle);
//		}
//		public async void DeleteDbArticle(int dbArticleId)
//		{
//			this.context.Entry(await this.context.DbArticles.FindAsync(dbArticleId)).State = EntityState.Deleted;
//		}

//		/// <summary>
//		/// <b>Be careful!</b>
//		/// This method is actually only changes the state of passed object to EntityState.Modified.
//		/// </summary>
//		public void UpdateDbArticle(DbArticle dbArticle)
//		{
//			this.context.Entry(dbArticle).State = EntityState.Modified;
//		}
//		public async void Save()
//		{
//			await this.context.SaveChangesAsync();
//		}

//		/* Kinda marked for deletion
//		private bool disposed = false;

//		protected virtual void Dispose(bool disposing)
//		{
//			if (!this.disposed)
//			{
//				if (disposing)
//				{
//					context.Dispose();
//				}
//			}
//			this.disposed = true;
//		}

//		public void Dispose()
//		{
//			Dispose(true);
//			GC.SuppressFinalize(this);
//		}
//		*/
//	}
//}
