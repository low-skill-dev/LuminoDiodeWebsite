using System;
using Utf8Json;
using RandomDataGenerator;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Update;
using NpgsqlTypes;
using Website.Models.UserModel;

namespace Website.Models.DocumentModel
{
	public class DbDocument
	{
		public int? Id { get; set; }
		public string Title { get; set; }
		public NpgsqlTsVector TitleTsVector { get; set; }
		public User Author { get; set; }
		public string[] Tags { get; set; }
		public long CreatedDateTime { get; set; }
		public byte[] Utf8JsonSerializedParagraphs { get; set; }

		public static DbDocument FromDocument(Document article) => new DbDocument
		{
			Id = article.Id,
			Title = article.Title,
			Author = article.Author,
			Tags = article.Tags,
			CreatedDateTime = article.CreatedDateTime.ToBinary(),
			Utf8JsonSerializedParagraphs = JsonSerializer.Serialize(article.Paragraphs)
		};
		public Document ToDocument() => new Document
		{
			Id = this.Id,
			Title = this.Title,
			Author = this.Author,
			Tags = this.Tags,
			CreatedDateTime = DateTime.FromBinary(this.CreatedDateTime),
			Paragraphs = JsonSerializer.Deserialize<DocumentParagraph[]>(this.Utf8JsonSerializedParagraphs)
		};
	}
}
