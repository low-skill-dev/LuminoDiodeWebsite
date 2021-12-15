using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Website
{
	public class Program
	{
		[DllImport("kernel32.dll")] public extern static bool AllocConsole();
		public static void Main(string[] args)
		{
			AllocConsole();
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
