using System.Linq;
using System.Collections.Generic;
using LuminoDiodeRandomDataGenerators;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DocumentModel
{
	public class DocumentParagraph
	{
		public WebText[]? TextParts;
		public string? String64Image;
		public string? ImageSubtext;
#if DEBUG
		public static DocumentParagraph GenerateRandom()
		{
			return new DocumentParagraph { TextParts = RandomDataGenerator.ArrayOf(WebText.GenerateRandom), String64Image = RandomDataGenerator.String() };
		}
#endif
	}
}
