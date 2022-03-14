using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Repository;
using Website.Services.SettingsProviders;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Website.Services
{
	public class RandomDataSeederService
	{
		private readonly WebsiteContext context;
		private readonly AppSettingsProvider SettingsProvider;
		public RandomDataSeederService(AppSettingsProvider SettingsProvider, WebsiteContext ctx)
		{
			this.context = ctx;
			this.SettingsProvider = SettingsProvider;
		}

		public void SeedData(bool SaveChanges = true)
		{
			/* Be aware!
			 * Seeding projects must be launched only if at least 1 user exists in DB.
			 */
			this.SeedData_DbDocuments();
			this.SeedData_Users();
			this.SeedData_Projects();
			this.context.SaveChanges();
		}

		private void SeedData_DbDocuments()
		{
			// UNSAFE!
			// Some SQL DBs, like SQL Server creates non-linear ids, it means that there can be ids like 1,2,1001,1003,9991 in the db with only 5 added items,
			// so this is kinda unstable code, but should work with postgres

			// this checks if there is at least 10 raws in the db
			// if (this.DbDocuments.Find(10000) != null && DoNotSeedIfDataExists) return;


			int NumToAdd = this.SettingsProvider.RandomDataSeederSP.SeedUntilAmountOfDocumentsLeesThen - this.context.DbDocuments.Count();
			if (NumToAdd < 0) NumToAdd = 0;
			var ToAddUsers = new Website.Models.DocumentModel.DbDocument[NumToAdd];
			int CurrCt = 0;

			for (int i = 0; i < ToAddUsers.Length; i++)
			{
				var docToAdd = Models.DocumentModel.DbDocument.FromDocument(Website.Models.DocumentModel.Document.GenerateRandom());
				this.context.DbDocuments.Add(docToAdd);
				if (CurrCt++ > 10*1000)
				{
					CurrCt = 0;
					this.context.SaveChanges();
				}
			}
			this.context.SaveChanges();
		}
		private void SeedData_Users()
		{
			int NumToAdd = this.SettingsProvider.RandomDataSeederSP.SeedUntilAmountOfUsersIsLeesThen - this.context.Users.Count();
			if (NumToAdd < 0) NumToAdd = 0;
			var ToAddUsers = new Website.Models.UserModel.User[NumToAdd];

			for (int i = 0; i < ToAddUsers.Length; i++)
			{
				var ToAdd = new Models.UserModel.User
				{
					//Id = null,
					EmailAdress = "testemail@gmail.com",
					FirstName = "Admin",
					LastName = "Adminovich"
				};
				this.context.Users.Add(ToAdd);
			}
			this.context.SaveChanges();
		}
		private void SeedData_Projects()
		{
			int NumToAdd = this.SettingsProvider.RandomDataSeederSP.SeedUntilAmountOfProjectsLeesThen - this.context.Projects.Count();
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
