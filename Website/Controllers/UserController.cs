using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;
using System.Threading;
using System.Threading.Tasks;


namespace Website.Controllers
{
	public class UserController : AControllerWithAuth
	{
		private readonly IServiceScopeFactory ScopeFactory;
		private readonly Website.Repository.WebsiteContext context;
		private readonly Website.Services.RecentDocumentsBackgroundService recentDocumentsProvider;
		public UserController(IServiceScopeFactory Services, Website.Services.RecentDocumentsBackgroundService documentsBackgroundService, SessionManager SM)
			: base(SM, Services.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>())
		{
			this.ScopeFactory = Services;
			this.context = Services.CreateScope().ServiceProvider.GetRequiredService<WebsiteContext>();
			this.recentDocumentsProvider = documentsBackgroundService;
		}

		[HttpGet]
		public ViewResult Summary()
		{
			return this.View();
		}
		
		[HttpGet] 
		public ActionResult Login()
		{
			if (base.AuthedUser != null) return new StatusCodeResult(409); // 409 "Conflict", already signed in
			else return View();
		}
		[HttpPost]
		public async Task<ActionResult> Login(Website.Models.Auth.LoginInfo LI)
		{
#if DEBUG
			//this.ScopeFactory.CreateScope().ServiceProvider.GetRequiredService<PasswordsService>().SetPassAndSaltForUser(1, "qwerty");
#endif

			var EmailPlainText = LI.EmailPlainText;
			var PasswordPlainText = LI.PasswordPlainText;
			Website.Models.UserModel.User? found;
			try {  found = await this.context.Users.FirstAsync(x => x.EmailAdress.Equals(EmailPlainText) && x.AuthHashedPassword!=null); }
			catch (System.InvalidOperationException) { found = null; }

			if (found == null) return new StatusCodeResult(500); // user not found
			if(this.ScopeFactory.CreateScope().ServiceProvider.GetRequiredService<PasswordsService>()
				.ConfirmPassword(PasswordPlainText, found.AuthHashedPassword, found.AuthPasswordSalt))
			{
				base.SM.CreateSession(found.Id, out var CreatedSessId);
				Response.Cookies.Append(SessionManager.SessionIdCoockieName, CreatedSessId);
				return View("Show", found);
			}

			return new StatusCodeResult(401); // 401 not authorized
		}
		
		[HttpGet]
		public async Task<IActionResult> Show(int Id)
		{
			Website.Models.UserModel.User? found;
			found = await this.context.Users.FindAsync(Id);

			if (found == null) return new StatusCodeResult(404); // user not found
			else return View(found);
		}
	}
}
