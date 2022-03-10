using System.ComponentModel.DataAnnotations;

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
		

		public int? Id { get; set; }= null!;
		public USER_TYPE UserType { get; set; } = USER_TYPE.Common;
		public string FirstName { get; set; }= null!;
		public string? LastName { get; set; }= null!;
		public string? AboutMe { get; set; }= null!;
		public string? TelegramLink { get; set; }= null!;
		public string? VkLink { get; set; }= null!;
		public string? City { get; set; }= null!;
		public string? PostalCode { get; set; }= null!;
		public string? String64_ProfileImage { get; set; }= null!;

		public string EmailAdress { get; set; }= null!;
		//public string AuthUserName { get; set; }= null!;
		public byte[] AuthHashedPassword { get; set; }= null!;
		public byte[] AuthPasswordSalt { get; set; }= null!;

		public string GetFullName()
		{
			if (this.LastName == null) return this.FirstName;
			else return this.FirstName + ' ' + this.LastName;
		}
	}
}
