using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using Website.Repository.Models;
using System;
using Website.Repository;
using Website.Models.ArticleModel;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Services
{
	public interface IArticleService
	{
		//IQueryable<DbArticle> GetAllDbArticles();
		Task<Article> GetArticleById(int id);
	}
	public class ArticleService : IArticleService
	{
		private readonly IDbArticleRepository dbArticleRepository;

		public ArticleService(IDbArticleRepository dbArticleRepository)
		{
			this.dbArticleRepository = dbArticleRepository;
		}
		public async Task<Article> GetArticleById(int id)
		{
			return (await this.dbArticleRepository.GetDbArticleById(id)).ToArticle();
		}
	}
}
