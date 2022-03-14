using Website.Models.UserModel;
using Website.Models.ProjectModel;

namespace Website.Models.ProjectsGroupModel
{
	public class ProjectsGroup
	{
		#pragma warning disable CS8618

		public int Id { get; set; }
		public User OwnerId { get; set; }
		public User[] AdminsId { get; set; }
		public Project[] OrderedProjectsId { get; set; }
		public string ShortDescription { get; set; }

		#pragma warning restore CS8618
	}
}
