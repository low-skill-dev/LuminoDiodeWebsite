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
		public int SeedIfAmountOfUsersIsLeesThen
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfAmountOfUsersIsLeesThen")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 3;
				}
			}
		}
		public int SeedIfAmountOfDocumentsLeesThen
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfAmountOfDocumentsLeesThen")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 100 * 1000;
				}
			}
		}
		public int SeedIfAmountOfProjectsLeesThen
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfAmountOfProjectsLeesThen")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 10 * 1000;
				}
			}
		}

		public int SeedIfAmountOfProjectsGroupsLeesThen
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfAmountOfProjectsGroupsLeesThen")
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