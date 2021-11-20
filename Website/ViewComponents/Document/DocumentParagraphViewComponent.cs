using Microsoft.AspNetCore.Mvc;
using Website.Models.DocumentModel;

namespace Website.ViewComponents.Document
{
	public class DocumentParagraphViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(Website.Models.DocumentModel.DocumentParagraph documentParagraph)
		{
			return View(documentParagraph);
		}

	}
}
