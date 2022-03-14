using NpgsqlTypes;
using System;
using Utf8Json;
using Website.Models.UserModel;

namespace Website.Models.DocumentModel
{
	public class DbDocument
	{
		public int Id { get; set; }
		public string Title { get; set; }= null!;
		public NpgsqlTsVector TitleTsVector { get; set; }= null!;
		public User Author { get; set; }= null!;
		public string[] Tags { get; set; }= null!;
		public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
		public byte[] Utf8JsonSerializedParagraphs { get; set; }= null!;


		public static DbDocument FromDocument(Document article) => new DbDocument
		{
			Id = article.Id,
			Title = article.Title,
			Author = article.Author,
			Tags = article.Tags,
			CreatedDateTime = article.CreatedDateTime,
			Utf8JsonSerializedParagraphs = JsonSerializer.Serialize(article.Paragraphs)
		};
		public Document ToDocument() => new Document
		{
			Id = this.Id,
			Title = this.Title,
			Author = this.Author,
			Tags = this.Tags,
			CreatedDateTime = this.CreatedDateTime,
			Paragraphs = JsonSerializer.Deserialize<DocumentParagraph[]>(this.Utf8JsonSerializedParagraphs)
		};
	}
}
