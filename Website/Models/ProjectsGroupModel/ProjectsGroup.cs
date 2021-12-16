namespace Website.Models.ProjectsGroupModel
{
	public class ProjectsGroup
	{
		public int Id { get; set; }
		public int OwnerId { get; set; }
		public int[] AdminsId { get; set; }
		public int[] OrderedProjectsId { get; set; }
		public string ShortDescription { get; set; }
	}
}
