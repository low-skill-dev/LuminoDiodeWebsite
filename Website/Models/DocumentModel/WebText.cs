using System;

namespace Website.Models.DocumentModel
{
	public class WebText
	{
		#pragma warning disable CS8618
		
		public string Text;
		public string? Link;
		public bool? IsBold;
		public bool? IsItalic;

		#pragma warning restore CS8618

#if DEBUG
		private static readonly Random rnd = new Random();
		public static WebText GenerateRandomProps(string OrigString)
		{
			return new WebText
			{
				Text = OrigString,
				Link = rnd.Next(0, 2) < 1 ? "www.google.com" : null,
				IsBold = rnd.Next(0, 2) < 1 ? true : false,
				IsItalic = rnd.Next(0, 2) < 1 ? true : false
			};

		}
#endif
	}
}
