namespace Website.Models.UserModel
{
	public class User
	{
		public enum USER_TYPE
		{
			Common = 0,
			Moderator = 1,
			Admin = 2
		}

		public int Id { get; set; }
		public USER_TYPE UserType { get; set; }
		public string FirstName { get; set; }
		public string? LastName { get; set; }
		public string? AboutMe { get; set; }
		public string? TelegramLink { get; set; }
		public string? VkLink { get; set; }
		public string? City { get; set; }
		public string? PostalCode { get; set; }
	}
}
