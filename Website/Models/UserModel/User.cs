using System;
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
		// this enum shows page where user should be redirected while entering the site
		public enum REGISTRATION_STAGE
		{
			EnteringName, // user needs to enter a name
			EnteringMetadata, // City, aboutme, telegram link e.t.c
			RegistrationCompleted
		}

		public int Id { get; set; }

		[EnumDataType(typeof(USER_TYPE))]
		public USER_TYPE UserType { get; set; } = USER_TYPE.Common;

		[DataType(DataType.Text)]
		[MinLength(1), MaxLength(50)]
		[Required(ErrorMessage = "Please enter a correct user name")]
		public string DisplayedName { get; set; } = "New User";

		[DataType(DataType.Text)]
		[MinLength(1), MaxLength(3000)]
		public string? AboutMe { get; set; } = null!;

		[DataType(DataType.Url)]
		[MinLength(1), MaxLength(50)]
		public string? TelegramLink { get; set; } = null!;

		[DataType(DataType.Url)]
		[MinLength(1), MaxLength(50)]
		public string? VkLink { get; set; } = null!;

		[DataType(DataType.Text)]
		[MinLength(1), MaxLength(50)]
		public string? City { get; set; } = null!;

		[DataType(DataType.PostalCode)]
		[MinLength(1), MaxLength(10)]
		public string? PostalCode { get; set; } = null!;

		public string? String64_ProfileImage { get; set; } = null!;

		[DataType(DataType.EmailAddress)]
		[MinLength(4), MaxLength(50)]
		[Required(ErrorMessage = "Please enter correct email address")]
		public string? EmailAdress { get; set; } = null!;

		public byte[]? AuthHashedPassword { get; set; } = null!;
		public byte[]? AuthPasswordSalt { get; set; } = null!;

		public DateTime RegistrationStartedDateTime { get; set; } = DateTime.UtcNow;
		public DateTime? RegistrationCompleteDateTime { get; set; } = null!;

		[EnumDataType(typeof(REGISTRATION_STAGE))]
		public REGISTRATION_STAGE RegistrationStage { get; set; } = REGISTRATION_STAGE.EnteringName;

		public string GetFullName()
		{
			if (this.LastName == null) return this.FirstName;
			else return this.FirstName + ' ' + this.LastName;
		}
		public string FirstName => this.DisplayedName;
		public string LastName => null;

		public void UpdateFromNameModel(NameModel NM)
		{
			this.DisplayedName = NM.FirstName + ' ' + NM.LastName;
		}
		public void UpdateFromMetadataModel(MetadataModel MM)
		{
			if (MM.AboutMe is not null)
			{
				this.AboutMe = MM.AboutMe;
			}
			if (MM.City is not null)
			{
				this.City = MM.City;
			}
			if (MM.TelegramLink is not null)
			{
				this.TelegramLink = MM.TelegramLink;
			}
			if (MM.PostalCode is not null)
			{
				this.PostalCode = MM.PostalCode;
			}
			if (MM.VkLink is not null)
			{
				this.VkLink = MM.VkLink;
			}
		}
	}
}
