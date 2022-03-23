namespace Website.Models.Auth
{
	public class LoginInfo
	{
		public string UserName { get; set; } //For registration only
		public string EmailPlainText { get; set; }
		public string PasswordPlainText { get; set; }
	}
}
