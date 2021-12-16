using Website.Models.DocumentModel;
using Website.Models.UserModel;

namespace Website.Models.ProjectsGroupModel
{
	public class ProjectsGroup
	{
		public int Id { get; set; }
		public User OwnerId { get; set; }
		public User[] Admins { get; set; }
		public DbDocument[] OrderedProjects { get; set; }
		public string ShortDescription { get; set; }
	}
}
