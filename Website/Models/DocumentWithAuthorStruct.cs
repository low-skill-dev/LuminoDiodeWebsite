using System;
using System.Runtime;

namespace Website.Models
{
	[Obsolete(null,true)]
	public struct DocumentWithAuthorStruct
	{
		public DocumentModel.Document Document { get; init; }
		public UserModel.User AuthorUser { get; init; }
	}
}
