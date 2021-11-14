using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Repository;
using Microsoft.EntityFrameworkCore;
using Website.Models.DocumentModel;

namespace Website.ViewComponents.Article
{
	public class WebTextViewComponent : ViewComponent
	{
		private string GetHtmlString()
		{
			lock (this.webText)
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
		private WebText webText;

		public WebTextViewComponent(Website.Models.DocumentModel.WebText webText) => this.webText = webText;

		public IViewComponentResult Invoke()
		{
			return View(model: this.GetHtmlString());
		}
	}
}
