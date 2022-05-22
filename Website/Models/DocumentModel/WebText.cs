using System;
using System.Text;

namespace Website.Models.DocumentModel
{
	public class WebText
	{
		public string Text = string.Empty;
		public string? Link;
		public bool? IsBold;
		public bool? IsItalic;

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

		public string ToHtmlString()
		{
			bool hasLink = !string.IsNullOrWhiteSpace(this.Link);
			bool isBold = this.IsBold ?? false;
			bool isItalic = this.IsItalic ?? false;

			StringBuilder sb = new();

			sb.Append("<div>");

			if (hasLink)
			{
				sb.Append("<a href=\'{this.Link}\'>");
			}
			if (isBold)
			{
				sb.Append("<strong>");
			}
			if (isItalic)
			{
				sb.Append("<i>");
			}

			sb.AppendLine(this.Text);

			if (hasLink)
			{
				sb.Append("</i>");
			}
			if (isBold)
			{
				sb.Append("</strong>");
			}
			if (isItalic)
			{
				sb.Append("</a>");
			}

			sb.Append("</div>");

			return sb.ToString();
		}
	}
}
