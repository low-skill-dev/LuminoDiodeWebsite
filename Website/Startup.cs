using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Repository;
using Website.Services;
using Website.Services.SettingsProviders;


namespace Website
{
	public class Startup
	{
		private readonly IConfiguration configuration;
		public Startup(IConfiguration config)
		{
			this.configuration = config;
		}

		private IServiceCollection services;
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddSingleton<Website.Services.RecentDocumentsBackgroundService>();
			services.AddSingleton<Website.Services.FrequentSearchRequestsService>();
			var AppSetProv = new AppSettingsProvider(this.configuration);
			services.AddSingleton<AppSettingsProvider>(AppSetProv);
			services.AddScoped<Website.Services.DocumentSearchService>();

			DbContextOptions<WebsiteContext> dbContextOptions = new DbContextOptions<WebsiteContext>();

			services.AddDbContext<WebsiteContext>(opts =>
			{
				opts.UseNpgsql(AppSetProv.ConnectionStringsP.DefaultNpgsqlConnection);
			});

			this.services = services;
		}


		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			#region StartServices
			app.ApplicationServices.GetService<RecentDocumentsBackgroundService>().StartAsync(new System.Threading.CancellationToken());
			app.ApplicationServices.GetService<FrequentSearchRequestsService>().StartAsync(new System.Threading.CancellationToken());
			#endregion


			app.UseStaticFiles();
			//app.UseStatusCodePages();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default", pattern: "{controller=Home}/{Action=Summary}/{Id?}");
			});
		}
	}
}
