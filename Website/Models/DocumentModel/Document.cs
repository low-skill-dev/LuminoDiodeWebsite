using System;
using System.Linq;
using Website.Models.UserModel;

namespace Website.Models.DocumentModel
{
	public class Document
	{

		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public User? Author { get; set; } = null!;
		public string[]? Tags { get; set; } = null!;
		public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedDateTime { get; set; } = DateTime.UtcNow;
		public DocumentParagraph[] Paragraphs { get; set; } = null!;
		public string? TextPrerenderedHtml { get; set; } = null!;
		public DateTime? PrerenderedHtmlCreationDateTime { get; set; } = null!;


		public override string ToString()
		{
			return Utf8Json.JsonSerializer.ToJsonString(this);
		}

		private static readonly Random rnd = new Random();
		public static Document GenerateRandom()
		{
			return new Document
			{
				Title = new RandomDataGenerator.Randomizers.RandomizerTextWords(new RandomDataGenerator.FieldOptions.FieldOptionsTextWords { }).Generate(),
				//Author = new UserModel.User() { Id=0 },
				Tags = new string[rnd.Next(1, 6)]
					.Select(x => new string(new RandomDataGenerator.Randomizers.RandomizerTextLipsum(new RandomDataGenerator.FieldOptions.FieldOptionsTextLipsum { }).Generate()
					.Split(' ')[rnd.Next(0, 7)].Where(x => char.IsLetterOrDigit(x)).ToArray())).ToArray(),
				CreatedDateTime = DateTime.UtcNow,
				Paragraphs = new DocumentParagraph[rnd.Next(2, 15)].Select(x => DocumentParagraph.GenerateRandom()).ToArray()
			};
		}

		public void CreatePrerender()
		{

		}
	}
}
