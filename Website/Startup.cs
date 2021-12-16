using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Repository;
using Website.Services;


namespace Website
{
	public class Startup
	{
		private IServiceCollection services;

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddCors();

			DbContextOptions<WebsiteContext> dbContextOptions = new DbContextOptions<WebsiteContext>();

			services.AddDbContext<WebsiteContext>(opts =>
			{
				opts.UseNpgsql("Server=localhost;Database=LuminodiodeWebsiteDb1;Password=qwerty;username=postgres");
			});

			services.AddSingleton<Website.Services.RecentDocumentsBackgroundService>();
			services.AddSingleton<Website.Services.FrequentSearchRequestsService>();
			services.AddScoped<Website.Services.DocumentSearchService>();
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
