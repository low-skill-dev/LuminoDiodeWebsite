using System.Linq;
using Website.Repository;
using Website.Services.SettingsProviders;

namespace Website.Services
{
	public class RandomDataSeederService
	{
		private readonly WebsiteContext context;
		private readonly RandomDataSeederSettingsProvider SettingsProvider;
		public RandomDataSeederService(AppSettingsProvider SettingsProvider, WebsiteContext ctx)
		{
			this.context = ctx;
			this.SettingsProvider = SettingsProvider.RandomDataSeederSP;
		}

		public void SeedData(bool SaveChanges = true)
		{
			/* Be aware!
			 * Seeding projects must be launched only if at least 1 user exists in DB.
			 */
			if (SettingsProvider.SeederIsEnabled)
			{
				this.SeedData_Users();
				this.SeedData_DbDocuments();
				this.SeedData_Projects();
				this.context.SaveChanges();
			}
		}

		private void SeedData_DbDocuments()
		{
			// UNSAFE!
			// Some SQL DBs, like SQL Server creates non-linear ids, it means that there can be ids like 1,2,1001,1003,9991 in the db with only 5 added items,
			// so this is kinda unstable code, but should work with postgres

			// this checks if there is at least 10 raws in the db
			// if (this.DbDocuments.Find(10000) != null && DoNotSeedIfDataExists) return;

			int NumToAdd = this.SettingsProvider.SeedIfAmountOfDocumentsLeesThen - this.context.DbDocuments.Count();
			if (NumToAdd < 0) NumToAdd = 0;
			var ToAddDocs = new Website.Models.DocumentModel.DbDocument[NumToAdd];

			for (int i = 0; i < ToAddDocs.Length; i++)
			{
				var docToAdd = Models.DocumentModel.DbDocument.FromDocument(Website.Models.DocumentModel.Document.GenerateRandom());
				docToAdd.Author = this.context.Users.First();
				this.context.DbDocuments.Add(docToAdd);
			}
			this.context.SaveChanges();
		}
		private void SeedData_Users()
		{
			int NumToAdd = this.SettingsProvider.SeedIfAmountOfUsersIsLeesThen - this.context.Users.Count();
			if (NumToAdd < 0) NumToAdd = 0;
			var ToAddUsers = new Website.Models.UserModel.User[NumToAdd];


			var startEmailIndex = this.context.Users.Count();
			for (int i = 0; i < ToAddUsers.Length; i++)
			{
				var ToAdd = new Models.UserModel.User
				{
					//Id = null,
					EmailAdress = $"testemail{startEmailIndex + i}@gmail.com",
					DisplayedName = "Admin",
				};
				this.context.Users.Add(ToAdd);
			}
			this.context.SaveChanges();
		}
		private void SeedData_Projects()
		{
			int NumToAdd = this.SettingsProvider.SeedIfAmountOfProjectsLeesThen - this.context.Projects.Count();
			if (NumToAdd < 0) NumToAdd = 0;

			var ToAddProjects = new Website.Models.ProjectModel.Project[NumToAdd];

			for (int i = 0; i < ToAddProjects.Length; i++)
			{
				var ToAdd = Models.ProjectModel.Project.GenerateRandom(this.context);
				this.context.Projects.Add(ToAdd);
			}
			this.context.SaveChanges();
		}
		private void SeedData_ProjectsGroups()
		{

		}
	}
}
