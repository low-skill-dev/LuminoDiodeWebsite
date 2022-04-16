using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Website.Models.DocumentModel
{
	public class DocumentCreation
	{
		private const string PleaseCorrectStr = "Please enter a correct value";

		[DataType(DataType.Text)]
		[Required(ErrorMessage = PleaseCorrectStr)]
		[MinLength(3), MaxLength(200)]
		[RegularExpression(@".*\S{3}.*")]
		[Display(Name = "Title")]
		public string Title { get; set; } = null!;

		[DataType(DataType.MultilineText)]
		[Required(ErrorMessage = PleaseCorrectStr)]
		[MinLength(200), MaxLength(300000)]
		// add user defined regex
		[Display(Name = "Document text")]
		public string Text { get; set; } = null!;

		public static DocumentCreation FromDocument(Website.Models.DocumentModel.Document Doc) =>
			new DocumentCreation { Title = Doc.Title, Text = string.Concat(Doc.Paragraphs.Select(x => string.Concat(x.TextParts?.Select(y => y.Text) ?? new string[0]))) };
	}
}
