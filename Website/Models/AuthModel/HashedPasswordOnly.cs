using System.ComponentModel.DataAnnotations;

namespace Website.Models.AuthModel
{
	public class HashedPasswordOnly
	{
		[DataType(DataType.Text)]
		[MinLength(5), MaxLength(50)]
		[RegularExpression(@".*[\S]{5}.*")] // at least 5 non-space chars
		[Required(ErrorMessage = "Please enter a password")]
		[Display(Name = "Password")]
		public string HashedPasswordString64 { get; set; }
	}
}
