using System.ComponentModel.DataAnnotations;

namespace Website.Models.DocumentModel
{
	public class DocumentCreation
	{
		private const string PleaseCorrectStr = "Please enter a correct value";

		[DataType(DataType.Text)]
		[Required(ErrorMessage = PleaseCorrectStr)]
		[MinLength(3),MaxLength(200)]
		[RegularExpression(@".*\S{3}.*")]
		[Display(Name="Title")]
		public string Title { get; set; } = null!;

		[DataType(DataType.MultilineText)]
		[Required(ErrorMessage = PleaseCorrectStr)]
		[MinLength(300), MaxLength(200)]
		[RegularExpression(@".*\S{300}.*")]
		[Display(Name = "Document text")]
		public string Text { get; set; } = null!;
	}
}
