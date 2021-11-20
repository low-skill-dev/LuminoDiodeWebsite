using Microsoft.AspNetCore.Mvc;

namespace Website.ViewComponents.Document
{
	public class DocumentCardViewComponent:ViewComponent
	{
		public IViewComponentResult Invoke(Website.Models.DocumentModel.Document doc, Website.Models.UserModel.User AuthorUser)
		{
			
			return View(new Models.DocumentWithAuthorStruct { Document = doc, AuthorUser = AuthorUser });
		}
	}
}
