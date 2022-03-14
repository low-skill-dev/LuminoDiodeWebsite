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
		public int SeedUntilAmountOfUsersIsLeesThen
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedUntilAmountOfUsersIsLeesThen")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 3;
				}
			}
		}
		public int SeedUntilAmountOfDocumentsLeesThen
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedUntilAmountOfDocumentsLeesThen")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 100*1000;
				}
			}
		}
		public int SeedUntilAmountOfProjectsLeesThen
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedUntilAmountOfProjectsLeesThen")
						.Get<int>();
				}
				catch (Exception)
				{
					// default
					return 10*1000;
				}
			}
		}

		public int SeedUntilAmountOfProjectsGroupsLeesThen
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedUntilAmountOfProjectsGroupsLeesThen")
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