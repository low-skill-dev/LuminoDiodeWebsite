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

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddSession();

			var AppSetProv = new AppSettingsProvider(this.configuration);
			services.AddSingleton<AppSettingsProvider>(AppSetProv);

			services.AddSingleton<AuthTockenServiceSettingsProvider>(AppSetProv.AuthTockenServiceSP);
			services.AddSingleton<FrequentSearchRequestsServiceSettingsProvider>(AppSetProv.FrequentSearchRequestsServiceSP);
			services.AddSingleton<PasswordsCryptographyServiceSettingsProvider>(AppSetProv.PasswordsCryptographyServiceSP);
			services.AddSingleton<RandomDataSeederSettingsProvider>(AppSetProv.RandomDataSeederSP);
			services.AddSingleton<RecentDocumentsBackgroundServiceSettingsProvider>(AppSetProv.RecentDocumentsBackgroundServiceSP);
			services.AddSingleton<RequestsFromIpCounterServiceSettingsProvider>(AppSetProv.RequestsFromIpCounterServiceSP);
			services.AddSingleton<SessionManagerServiceSettingsProvider>(AppSetProv.SessionManagerServiceSP);

			services.AddSingleton<Website.Services.RecentDocumentsBackgroundService>();
			services.AddSingleton<Website.Services.FrequentSearchRequestsService>();
			services.AddSingleton<Website.Services.SessionManager>();
			services.AddSingleton<Website.Services.RequestsFromIpCounterService>();
			services.AddSingleton<Website.Services.AuthTockenService>();
			services.AddSingleton<Website.Services.RandomDataSeederService>();
			services.AddScoped<Website.Services.DocumentSearchService>();
			services.AddScoped<Website.Services.PasswordsService>();

			services.AddDbContext<WebsiteContext>(opts =>
			{
				opts.UseNpgsql(AppSetProv.ConnectionStringsP.DefaultNpgsqlConnection);
			});
		}


		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.ApplicationServices.GetRequiredService<RecentDocumentsBackgroundService>().StartAsync(new System.Threading.CancellationToken());
			app.ApplicationServices.GetRequiredService<FrequentSearchRequestsService>().StartAsync(new System.Threading.CancellationToken());
			app.ApplicationServices.GetRequiredService<SessionManager>().StartAsync(new System.Threading.CancellationToken());
			app.ApplicationServices.GetRequiredService<RandomDataSeederService>().SeedData();

			app.UseStaticFiles();
			app.UseRouting();
			app.UseSession();

			app.UseMiddleware<Website.Middleware.AntispamMiddleware>();
			app.UseMiddleware<Website.Middleware.AuthenticationMiddleware>();
			app.UseMiddleware<Website.Middleware.RegistrationStepsMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default", pattern: "{controller=Home}/{Action=Summary}/{Id?}");
			});

		}
	}
}