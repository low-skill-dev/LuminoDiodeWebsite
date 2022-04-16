using System.ComponentModel.DataAnnotations;

namespace Website.Models.Auth
{
	public class LoginInfo
	{
		[DataType(DataType.EmailAddress)]
		[MinLength(5), MaxLength(50)]
		[RegularExpression(@".*[\S]{5}.*")] // at least 5 non-space chars
		[Required(ErrorMessage = "Please enter a correct email address")]
		[Display(Name = "Email")]
		public string EmailPlainText { get; set; }

		[DataType(DataType.Password)]
		[MinLength(8, ErrorMessage = "Please use a password that is at least 8 characters long")]
		[MaxLength(128, ErrorMessage = "Your password is too long")]
		[Required(ErrorMessage = "Please enter a correct password")]
		[Display(Name = "Password")]

		public string PasswordPlainText { get; set; }
	}
}
