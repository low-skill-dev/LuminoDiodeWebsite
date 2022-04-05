using System.ComponentModel.DataAnnotations;
using System;

namespace Website.Models.UserModel
{
	public class MetadataModel
	{
		[DataType(DataType.Text)]
		[MinLength(5), MaxLength(3000)]
		[RegularExpression(@".*\S{5}.*")] // at least 5 non-space chars
		[Display(Name ="About me")]
		
		public string? AboutMe { get; set; } = null!;

		[DataType(DataType.Text)]
		[MinLength(5), MaxLength(50)]
		[RegularExpression(@"\s*[t][.][m][e][/]\S+\s*")] // *t.me/?
		[Display(Name = "t.me link")]
		public string? TelegramLink { get; set; } = null!;

		[DataType(DataType.Text)]
		[MinLength(5), MaxLength(50)]
		[RegularExpression(@"\s*[v][k][.][c][o][m][/]\S?")] // *vk.com/?
		[Display(Name = "vk.com link")]
		public string? VkLink { get; set; } = null!;

		[DataType(DataType.Text)]
		[MinLength(1), MaxLength(50)]
		[RegularExpression(@".*\S{5}.*")] // at least 1 non-space chars
		[Display(Name = "City")]
		public string? City { get; set; } = null!;

		[DataType(DataType.PostalCode)]
		[MinLength(3), MaxLength(10)]
		[RegularExpression(@".*\S{3}.*")] // at least 3 non-space chars
		[Display(Name = "Postal code")]
		public string? PostalCode { get; set; } = null!;
	
	
		public void TrimAllFields()
		{
			if(AboutMe is not null)
			this.AboutMe = this.AboutMe.Trim();
			if (TelegramLink is not null)
				this.TelegramLink = this.TelegramLink.Trim();
			if (VkLink is not null)
				this.VkLink = this.VkLink.Trim();
			if (City is not null)
				this.City = this.City.Trim();
			if (PostalCode is not null)
				this.PostalCode = this.PostalCode.Trim();
		}
	}
}
