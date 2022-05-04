using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Repository;
using Website.Services;
using Website.Services.SettingsProviders;
using Microsoft.AspNetCore.Mvc;


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
			services.AddSession();

			var AppSetProv = new AppSettingsProvider(this.configuration);
			services.AddSingleton<AppSettingsProvider>(AppSetProv);
			services.AddSingleton<Website.Services.RecentDocumentsBackgroundService>();
			services.AddSingleton<Website.Services.FrequentSearchRequestsService>();
			services.AddSingleton<Website.Services.SessionManager>();
			services.AddSingleton<Website.Services.RequestsFromIpCounterService>();
			services.AddScoped<Website.Services.DocumentSearchService>();
			services.AddScoped<Website.Services.PasswordsService>();
			services.AddScoped<Website.Services.RandomDataSeederService>();

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

			app.ApplicationServices.GetService<RecentDocumentsBackgroundService>().StartAsync(new System.Threading.CancellationToken());
			app.ApplicationServices.GetService<FrequentSearchRequestsService>().StartAsync(new System.Threading.CancellationToken());
			app.ApplicationServices.GetService<SessionManager>().StartAsync(new System.Threading.CancellationToken());

			app.UseStaticFiles();
			app.UseRouting();
			app.UseSession();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default", pattern: "{controller=Home}/{Action=Summary}/{Id?}");
			});
		}
	}
}
