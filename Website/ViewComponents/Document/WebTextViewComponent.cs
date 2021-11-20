using Microsoft.AspNetCore.Mvc;
using Website.Models.DocumentModel;

namespace Website.ViewComponents.Document
{
	public class WebTextViewComponent : ViewComponent
	{
		private static string GetHtmlString(Website.Models.DocumentModel.WebText webText)
		{
			lock (webText)
			{
				string Out = string.Empty;

				if (!string.IsNullOrWhiteSpace(webText.Link))
				{
					Out += $"<a href=\"{webText.Link}\">.";
				}
				if (webText.IsBold ?? false)
				{
					Out += "<strong>";
				}
				if (webText.IsItalic ?? false)
				{
					Out += "<i>";
				}

				Out += webText.Text;

				if (webText.IsItalic ?? false)
				{
					Out += "</i>";
				}
				if (webText.IsBold ?? false)
				{
					Out += "</strong>";
				}
				if (!string.IsNullOrWhiteSpace(webText.Link))
				{
					Out += $"</a";
				}

				return Out;
			}
		}

		public IViewComponentResult Invoke(Website.Models.DocumentModel.WebText webText)
		{
			return View(model: GetHtmlString(webText));
		}
	}
}
