using NpgsqlTypes;
using System;
using Utf8Json;
using Website.Models.UserModel;

namespace Website.Models.DocumentModel
{
	public class DbDocument
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public NpgsqlTsVector TitleTsVector { get; set; } = null!;
		public User? Author { get; set; }
		public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedDateTime { get; set; } = DateTime.UtcNow;
		public byte[] Utf8JsonSerializedParagraphs { get; set; } = null!;
		public string? TextPrerenderedHtml { get; set; } = null!;
		public DateTime? PreRenderedHtmlCreationDateTime { get; set; } = null!;


		public static DbDocument FromDocument(Document article) => new DbDocument
		{
			Id = article.Id,
			Title = article.Title,
			Author = article.Author,
			CreatedDateTime = article.CreatedDateTime,
			UpdatedDateTime = article.UpdatedDateTime,
			Utf8JsonSerializedParagraphs = JsonSerializer.Serialize(article.Paragraphs),
			TextPrerenderedHtml = article.TextPrerenderedHtml,
			PreRenderedHtmlCreationDateTime = article.PrerenderedHtmlCreationDateTime
		};
		public Document ToDocument() => new Document
		{
			Id = this.Id,
			Title = this.Title,
			Author = this.Author,
			CreatedDateTime = this.CreatedDateTime,
			UpdatedDateTime = this.UpdatedDateTime,
			Paragraphs = JsonSerializer.Deserialize<DocumentParagraph[]>(this.Utf8JsonSerializedParagraphs),
			TextPrerenderedHtml = this.TextPrerenderedHtml,
			PrerenderedHtmlCreationDateTime = this.PreRenderedHtmlCreationDateTime			
		};
	}
}
