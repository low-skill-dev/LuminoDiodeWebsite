namespace Website.Models.ProjectModel
{
	public class Project
	{
		public enum PROJECT_TYPE
		{
			Common = 0
		}
		public int Id { get; set; }
		public PROJECT_TYPE ProjectType { get; set; }
		public string ShortDescription { get; set; }
		public int OwnerId { get; set; }
		public int[] AdminsId { get; set; }
		public int[] OrderedDocumentsId { get; set; }
	}
}
