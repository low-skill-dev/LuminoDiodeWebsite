using System.ComponentModel.DataAnnotations;
using System;


namespace Website.Models.UserModel
{
	public class NameModel
	{
		[DataType(DataType.Text)]
		[MinLength(1), MaxLength(50)]
		[RegularExpression(@".*[\S]{1,50}.*")] // at least 1 non-space chars
		[Required(ErrorMessage = "Please enter a correct user name")]
		[Display(Name = "First name")]
		public string FirstName { get; set; } = null!;

		[DataType(DataType.Text)]
		[MinLength(1), MaxLength(50)]
		[RegularExpression(@".*[\S]{1,50}.*")] // at least 1 non-space chars
		[Required(ErrorMessage = "Please enter a correct user name")]
		[Display(Name ="Last name")]
		public string LastName { get; set; } = null!;

		public void TrimAllField()
		{
			this.FirstName = this.FirstName.Trim();
			this.LastName = this.LastName.Trim();
		}
	}
}
