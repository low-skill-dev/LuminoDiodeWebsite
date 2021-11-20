namespace Website.Models
{
	public struct DocumentWithAuthorStruct
	{
		public DocumentModel.Document Document { get; init; }
		public UserModel.User AuthorUser { get; init; }
	}
}
