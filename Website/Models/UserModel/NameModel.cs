using System.ComponentModel.DataAnnotations;
using System;


namespace Website.Models.UserModel
{
	public class NameModel
	{
		private const string PleaseCorrectStr = "Please enter a correct value";

		[DataType(DataType.Text)]
		[MinLength(1), MaxLength(50)]
		[RegularExpression(@".*\S{1}.*",ErrorMessage = PleaseCorrectStr)] // at least 1 non-space chars
		[Required(ErrorMessage = PleaseCorrectStr)]
		[Display(Name = "First name")]
		public string FirstName { get; set; } = null!;

		[DataType(DataType.Text)]
		[MinLength(1), MaxLength(50)]
		[RegularExpression(@".*\S{1}.*",ErrorMessage = PleaseCorrectStr)] // at least 1 non-space chars
		[Required(ErrorMessage = PleaseCorrectStr)]
		[Display(Name ="Last name")]
		public string LastName { get; set; } = null!;

		public void TrimAllField()
		{
			this.FirstName = this.FirstName.Trim();
			this.LastName = this.LastName.Trim();
		}
	}
}
