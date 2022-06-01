using Microsoft.Extensions.Configuration;
using System;

namespace Website.Services.SettingsProviders
{
	public class RandomDataSeederSettingsProvider
	{
		protected readonly IConfiguration config;
		public RandomDataSeederSettingsProvider(IConfiguration configuration)
		{
			this.config = configuration;
		}

		public bool SeederIsEnabled
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeederIsEnabled")
						.Get<bool>();
				}
				catch (Exception)
				{
					// default
					return false;
				}
			}
		}
		public int SeedIfQuantityOfUsersIsLessThan
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfQuantityOfUsersIsLessThan")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 3;
				}
			}
		}
		public int SeedIfQuantityOfDocumentsIsLessThan
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfQuantityOfDocumentsIsLessThan")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 100 * 1000;
				}
			}
		}
		public int SeedIfQuantityOfProjectsIsLessThan
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfQuantityOfProjectsIsLessThan")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 10 * 1000;
				}
			}
		}

		public int SeedIfQuantityOfProjectsGroupsIsLessThan
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfQuantityOfProjectsGroupsIsLessThan")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 10;
				}
			}
		}
	}
}