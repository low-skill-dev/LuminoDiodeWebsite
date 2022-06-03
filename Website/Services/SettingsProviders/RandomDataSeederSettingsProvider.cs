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

		public virtual bool SeederIsEnabled
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeederIsEnabled")
						.Get<bool>();
				}
				catch
				{
					// default
					return false;
				}
			}
		}
		public virtual int SeedIfQuantityOfUsersIsLessThan
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfQuantityOfUsersIsLessThan")
						.Get<int>();
				}
				catch
				{
					// default
					return 3;
				}
			}
		}
		public virtual int SeedIfQuantityOfDocumentsIsLessThan
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfQuantityOfDocumentsIsLessThan")
						.Get<int>();
				}
				catch
				{
					// default
					return 100 * 1000;
				}
			}
		}
		public virtual int SeedIfQuantityOfProjectsIsLessThan
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfQuantityOfProjectsIsLessThan")
						.Get<int>();
				}
				catch
				{
					// default
					return 10 * 1000;
				}
			}
		}

		public virtual int SeedIfQuantityOfProjectsGroupsIsLessThan
		{
			get
			{
				try
				{
					return this.config
						.GetRequiredSection("DataSeederSettings:SeedIfQuantityOfProjectsGroupsIsLessThan")
						.Get<int>();
				}
				catch
				{
					// default
					return 10;
				}
			}
		}
	}
}