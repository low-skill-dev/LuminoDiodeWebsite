using Microsoft.EntityFrameworkCore;

namespace Website.Repository
{
	public class WebsiteContext : DbContext
	{
		public WebsiteContext(DbContextOptions<WebsiteContext> options) : base(options)
		{
			this.Database.Migrate();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Website.Models.DocumentModel.DbDocument>()
				.HasGeneratedTsVectorColumn(p => p.TitleTsVector, "english", p => new { p.Title })
				.HasIndex(p => p.TitleTsVector)
				.HasMethod("GIN");

			modelBuilder.Entity<Website.Models.UserModel.User>()
				.HasAlternateKey(u => u.EmailAdress);
		}

		public DbSet<Website.Models.DocumentModel.DbDocument> DbDocuments { get; set; } = null!;
		public DbSet<Website.Models.UserModel.User> Users { get; set; } = null!;
		public DbSet<Website.Models.ProjectModel.Project> Projects { get; set; } = null!;
		public DbSet<Website.Models.ProjectsGroupModel.ProjectsGroup> ProjectsGroups { get; set; } = null!;
	}
}
