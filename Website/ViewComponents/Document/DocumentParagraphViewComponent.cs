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
    public class DocumentParagraphViewComponent:ViewComponent
    {
		private WebText webText;
		public DocumentParagraphViewComponent(Website.Models.DocumentModel.WebText webText) => this.webText = webText;
		
		public IViewComponentResult Invoke(Website.Models.DocumentModel.WebText webText)
		{
			return View(webText);
		}

	}
}
