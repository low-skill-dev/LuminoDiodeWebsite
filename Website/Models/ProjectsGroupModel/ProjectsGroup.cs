using Website.Models.UserModel;
using Website.Models.ProjectModel;

namespace Website.Models.ProjectsGroupModel
{
	public class ProjectsGroup
	{
		public int Id { get; set; }
		public User OwnerId { get; set; }
		public User[] AdminsId { get; set; }
		public Project[] OrderedProjectsId { get; set; }
		public string ShortDescription { get; set; }
	}
}
