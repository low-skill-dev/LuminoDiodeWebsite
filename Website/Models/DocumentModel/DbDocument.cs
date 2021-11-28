﻿using System;
using Utf8Json;

namespace Website.Models.DocumentModel
{
	public class DbDocument
	{
		public int? Id { get; set; }
		public string Title { get; set; }
		public int AuthorUserId { get; set; }
		public string[] Tags { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public byte[] Utf8JsonSerializedParagraphs { get; set; }

		public static DbDocument FromDocument(Document article) => new DbDocument
		{
			Id = article.Id ?? null,
			Title = article.Title,
			AuthorUserId = article.AuthorUserId,
			Tags = article.Tags,
			CreatedDateTime = article.CreatedDateTime,
			Utf8JsonSerializedParagraphs = JsonSerializer.Serialize(article.Paragraphs)
		};
		public Document ToDocument() => new Document
		{
			Id = this.Id,
			Title = this.Title,
			AuthorUserId = this.AuthorUserId,
			Tags = this.Tags,
			CreatedDateTime = this.CreatedDateTime,
			Paragraphs = JsonSerializer.Deserialize<DocumentParagraph[]>(this.Utf8JsonSerializedParagraphs)
		};
	}
}