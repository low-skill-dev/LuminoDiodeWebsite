using Website.Models.ArticleModel;
using Utf8Json;
using System;

namespace Website.Repository.Models
{
	public class DbArticle
	{
		public int? Id { get; set; }
		public string Title { get; set; }
		public int AuthorUserId { get; set; }
		public string[] Tags { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public byte[] Utf8JsonSerializedParagraphs { get; set; }

		public static DbArticle FromArticle(Article article) => new DbArticle
		{
			Id = article.Id?? null,
			Title = article.Title,
			AuthorUserId = article.AuthorUserId,
			Tags = article.Tags,
			CreatedDateTime = article.CreatedDateTime,
			Utf8JsonSerializedParagraphs = JsonSerializer.Serialize(article.Paragraphs)
		};
		public Article ToArticle() => new Article
		{
			Id = this.Id,
			Title = this.Title,
			AuthorUserId = this.AuthorUserId,
			Tags = this.Tags,
			CreatedDateTime= this.CreatedDateTime,
			Paragraphs = JsonSerializer.Deserialize<ArticleParagraph[]>(this.Utf8JsonSerializedParagraphs)
		};
	}
}
