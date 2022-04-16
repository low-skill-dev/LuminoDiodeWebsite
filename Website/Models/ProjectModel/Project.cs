using System.Collections.Generic;
using System.Linq;
using Website.Models.DocumentModel;
using Website.Models.UserModel;

namespace Website.Models.ProjectModel
{
	public class Project
	{
		public enum PROJECT_TYPE
		{
			Common = 0
		}

#pragma warning disable CS8618

		public int Id { get; set; }
		public PROJECT_TYPE ProjectType { get; set; }
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		public User Owner { get; set; }
		public User[] Admins { get; set; }
		public IEnumerable<DbDocument> OrderedDocumentsId { get; set; }

#pragma warning restore CS8618

		public static Project GenerateRandom(Website.Repository.WebsiteContext documentsSourceContext)
		{

			return new Project
			{
				ProjectType = PROJECT_TYPE.Common,
				Name = "I am a test project",
				ShortDescription = "Test project desciption",
				Owner = documentsSourceContext.Users.First(),
				Admins = new User[0],
				// Doc ID is never null cause of it has been loaded from DB
				OrderedDocumentsId = documentsSourceContext.DbDocuments.Take(10).ToList()
			};
		}
	}
}
