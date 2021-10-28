using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using LuminoDiodeRandomDataGenerators;
using System;
using Utf8Json;

namespace Website.Models.ArticleModel
{
	public class Article 
	{
		public int? Id { get; set; }
		public string Title {  get; set; }
		public int AuthorUserId { get; set; }
		public string[] Tags { get; set; }
		public DateTime CreatedDateTime {  get; set; }
		public ArticleParagraph[] Paragraphs { get; set; }

#if DEBUG
		public static Article GenerateRandom()
		{
			return new Article { Title = RandomDataGenerator.String(), Tags = RandomDataGenerator.ArrayOf(RandomDataGenerator.String, 5), Paragraphs = RandomDataGenerator.ArrayOf(ArticleParagraph.GenerateRandom) };
		}
#endif
		public override string ToString()
		{
			return Utf8Json.JsonSerializer.ToJsonString(this);
		}
	}
}
