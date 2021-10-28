using System.Linq;
using System.Collections.Generic;
using LuminoDiodeRandomDataGenerators;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.ArticleModel
{
	public class ArticleParagraph
	{
		public WebText[]? TextParts;
		public string? String64Image;
		public string? ImageSubtext;
#if DEBUG
		public static ArticleParagraph GenerateRandom()
		{
			return new ArticleParagraph { TextParts = RandomDataGenerator.ArrayOf(WebText.GenerateRandom), String64Image = RandomDataGenerator.String() };
		}
#endif
	}
}
