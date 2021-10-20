using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Website.Models.ArticleModel
{
	public class Article 
	{
		public int Id { get; set; }
		public string Title {  get; set; }
		public int AuthorUserId { get; set; }
		public string[] Tags { get; set; }

		public List<IArticleBodyPart> BodyParts;
	}
}
