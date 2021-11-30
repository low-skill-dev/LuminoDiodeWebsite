using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Website.Repository
{
	public class WebsiteContext : DbContext
	{
		public WebsiteContext(DbContextOptions<WebsiteContext> options) : base(options)
		{
			//Database.EnsureDeleted();
			Database.EnsureCreated();
			//Database.Migrate();

#if DEBUG
			SeedData();
#endif
		}
		public DbSet<Website.Models.DocumentModel.DbDocument> DbDocuments { get; set; }
		public DbSet<Website.Models.UserModel.User> Users { get; set; }
		public DbSet<Website.Models.ProjectModel.Project> Projects { get; set; }
		public DbSet<Website.Models.ProjectsGroupModel.ProjectsGroup> ProjectsGroups { get; set; }

#if DEBUG
		private void SeedData(bool SaveChanges = true)
		{
			SeedData_DbDocuments();

			if (SaveChanges) this.SaveChanges();
		}

		private void SeedData_DbDocuments(bool DoNotSeedIfDataExists=true)
		{
			// UNSAFE!
			// Some SQL DBs, like SQL Server creates non-linear ids, it means that there can be ids like 1,2,1001,1003,9991 in the db with only 5 added items,
			// so this is kinda unstable code, but should work with postgres

			// this checks if there is at least 10 raws in the db
			if (this.DbDocuments.Find(10) != null && DoNotSeedIfDataExists) return;

			var docsToAdd = new object[10].Select(x => Models.DocumentModel.DbDocument.FromDocument(Website.Models.DocumentModel.Document.GenerateRandom()));
			this.DbDocuments.AddRange(docsToAdd);
			this.UpdateRange(docsToAdd);
		}
		private  void SeedData_Users()
		{

		}
		private  void SeedData_Projects()
		{

		}
		private  void SeedData_ProjectsGroups()
		{

		}
#endif

	}
}
