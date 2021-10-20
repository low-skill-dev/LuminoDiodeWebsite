using System.Text.Json.Serialization;
using System.Text.Json;

namespace Website.Models.ArticleModel
{
	public class ArticleImage: IArticleBodyPart
	{	
		public string String64Image { get; set; }
        public string? ImageSubtext { get; set; }
	}
}
