using Microsoft.AspNetCore.Mvc;
using Website.Models.DocumentModel;



namespace Website.ViewComponents.Document
{
	public class DocumentViewComponent : ViewComponent
	{


		private Website.Models.DocumentWithAuthorStruct DocumentWithAuthor;
		public DocumentViewComponent(Website.Models.DocumentModel.Document doc, Website.Models.UserModel.User AuthorUser) 
			=> this.DocumentWithAuthor = new Models.DocumentWithAuthorStruct { Document = doc, AuthorUser = AuthorUser };
		public IViewComponentResult Invoke()
		{
			return View(this.DocumentWithAuthor);
		}
		public IViewComponentResult InvokeAsShortCard()
		{
			return View("AsShortCard",this.DocumentWithAuthor);
		}
	}
}
