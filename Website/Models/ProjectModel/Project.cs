using System;
using System.Linq;

namespace Website.Models.ProjectModel
{
	public class Project
	{
		public enum PROJECT_TYPE
		{
			Common = 0
		}
		public int? Id { get; set; }
		public PROJECT_TYPE ProjectType { get; set; }
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		public int OwnerId { get; set; }
		public int[] AdminsId { get; set; }
		public int[] OrderedDocumentsId { get; set; }

		public static Project GenerateRandom(Website.Repository.WebsiteContext documentsSourceContext)
		{
			Random rnd = new Random();

			var Docs = documentsSourceContext.DbDocuments.Select(x => x.Id).ToList();

			var RandomlyShuffeledIndeces = Enumerable.Range(0, (Docs.Count / rnd.Next(1, 5))).OrderBy(x => rnd.Next());

			return new Project
			{
				ProjectType = PROJECT_TYPE.Common,
				Name = "I am a test project",
				ShortDescription = "Test project desciption",
				OwnerId = 1,
				AdminsId = new int[0],
				// Doc ID is never null cause of it has been loaded from DB
				OrderedDocumentsId = RandomlyShuffeledIndeces.Select(x => Docs[x].Value).ToArray()
			};
		}
	}
}
