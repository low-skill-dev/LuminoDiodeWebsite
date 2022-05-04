using System.ComponentModel.DataAnnotations;

namespace Website.Models.Auth
{
	public class LoginOnly
	{
		[DataType(DataType.EmailAddress)]
		[MinLength(5), MaxLength(50)]
		[RegularExpression(@".*[\S]{5}.*")] // at least 5 non-space chars
		[Required(ErrorMessage = "Please enter a correct email address")]
		[Display(Name = "Email")]
		public string EmailPlainText { get; set; }


		public void TrimSelf()
		{
			this.EmailPlainText = this.EmailPlainText.Trim();
		}
	}
}
