using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Website.Repository;
using Website.Services;
using System.Threading;
using System.Threading.Tasks;
using Website.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Website.Controllers
{
	public class UserController : AMyController
	{
		protected readonly IServiceScopeFactory ScopeFactory;
		protected readonly Website.Services.RecentDocumentsBackgroundService recentDocumentsProvider;
		protected readonly Website.Services.PasswordsService passwordsService;
		public UserController(IServiceScopeFactory Services, Website.Services.RecentDocumentsBackgroundService documentsBackgroundService, SessionManager SM)
			: base(Services)
		{
			this.ScopeFactory = Services;
			var sp = Services.CreateScope().ServiceProvider;
			this.recentDocumentsProvider = sp.GetRequiredService<RecentDocumentsBackgroundService>();
			this.passwordsService = sp.GetRequiredService<PasswordsService>();
		}

		[HttpGet]
		public ViewResult Summary()
		{
			return this.View();
		}

		[HttpGet]
		public async Task<IActionResult> Show(int Id)
		{
			Website.Models.UserModel.User? found;
			found = await this.context.Users.FindAsync(Id);

			if (found == null) return new StatusCodeResult(404); // user not found
			else return View(found);
		}

		[HttpGet]
		public IActionResult Login()
		{
			if (base.AuthedUser != null) return new StatusCodeResult(409); // 409 "Conflict", already signed in
			else return View();
		}
		[HttpPost]
		public async Task<ActionResult> Login([Bind]Website.Models.Auth.LoginInfo LI)
		{
			if (!ModelState.IsValid)
				return View(LI);

			var found = await this.context.Users.FirstOrDefaultAsync(x => x.EmailAdress==LI.EmailPlainText);

			if (found == null)
			{
				base.AddAlertToPageTop(new Alert("User not found", Alert.ALERT_TYPE.Danger));
				return View(LI);
			}
			if(found.AuthHashedPassword is null || found.AuthPasswordSalt is null)
			{
				return new StatusCodeResult(500); // should never be returned in prod
			}

			if (passwordsService.ConfirmPassword(LI.PasswordPlainText, found.AuthHashedPassword, found.AuthPasswordSalt))
			{
				base.SM.CreateSession(found.Id, out var CreatedSessId);
				Response.Cookies.Append(SessionManager.SessionIdCoockieName, CreatedSessId);
				/* Блядина ASP не умеет в сериализацию пустых списков.
				 * Так что если вдруг вам пришло в голову не отображать никаких алертов вверху страницы,
				 * то вы обязательно должны обНУЛЛить массив с ними, ни в коем случае не передавать пустые списки!
				 */
				//TempData["PageTopAlerts"] = null; // пофикшено нахуй
				return RedirectToAction("Show", "User", new { id = found.Id });
			}
			else
			{
				base.AddAlertToPageTop(new Alert("Wrong email or password" ,Alert.ALERT_TYPE.Danger));
				return View();
			}

			return new StatusCodeResult(500); // unknown error
		}

		[HttpGet]
		public ActionResult Logout()
		{
			Response.Cookies.Delete(SessionManager.SessionIdCoockieName);
			return RedirectToAction("Summary", "Home");
		}

		[HttpGet]
		public ActionResult Register()
		{
			if (base.AuthedUser != null) return new StatusCodeResult(409); // 409 "Conflict", already signed in
			else return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(Website.Models.Auth.LoginInfo LI)
		{
			if (!ModelState.IsValid)
			{
				return View(LI);
			}

			var EmailPlainText = LI.EmailPlainText;
			var PasswordPlainText = LI.PasswordPlainText;

			Website.Models.UserModel.User? found;
			found = await this.context.Users.FirstOrDefaultAsync(x =>  x.EmailAdress.Equals(EmailPlainText));

			if (found != null)
			{
				base.AddAlertToPageTop(new Alert("This email is already occupied", Alert.ALERT_TYPE.Danger));
				return View("Login");
			}

			else
			{
				var hashedpass = passwordsService.HashPassword(PasswordPlainText, out var Salt);

				var UserToAdd= new Models.UserModel.User
				{
					EmailAdress = EmailPlainText,
					AuthHashedPassword = hashedpass,
					AuthPasswordSalt = Salt,
					DisplayedName = "New User",
				};
				context.Users.Add(UserToAdd);
				await context.SaveChangesAsync();
				UserToAdd.DisplayedName = "user" + UserToAdd.Id;
				await context.SaveChangesAsync();
				base.AddAlertToPageTop(new Alert("Account created. Log in.",Alert.ALERT_TYPE.Info));
				return View("Login");
			}
		}

		[HttpGet]
		public IActionResult RegistrationStageEnteringName()
		{
			if (this.AuthedUser == null)
				return new StatusCodeResult(401); // 401 Unauthorized
			if (this.AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.EnteringName)
				return new StatusCodeResult(422); // 422 Unprocessable Entity

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> RegistrationStageEnteringName(Website.Models.UserModel.NameModel NM)
		{
			if (this.AuthedUser == null)
				return new StatusCodeResult(401); // 401 Unauthorized
			if (this.AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.EnteringName)
				return new StatusCodeResult(422); // 422 Unprocessable Entity


			if (!ModelState.IsValid)
				return View(NM);

			this.AuthedUser.UpdateFromNameModel(NM);
			this.AuthedUser.RegistrationStage = Models.UserModel.User.REGISTRATION_STAGE.EnteringMetadata;
			await context.SaveChangesAsync();

			return RedirectToAction("RegistrationStageEnteringMetadata", "User");
		}
		[HttpGet]
		public IActionResult RegistrationStageEnteringMetadata()
		{
			if (this.AuthedUser == null)
				return new StatusCodeResult(401); // 401 Unauthorized
			if (this.AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.EnteringMetadata)
				return new StatusCodeResult(422); // 422 Unprocessable Entity

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> RegistrationStageEnteringMetadata(Website.Models.UserModel.MetadataModel MM)
		{
			if (this.AuthedUser == null)
				return new StatusCodeResult(401); // 401 Unauthorized
			if (this.AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.EnteringMetadata)
				return new StatusCodeResult(422); // 422 Unprocessable Entity

			MM.TrimAllFields();
			if (!ModelState.IsValid)
				return View(MM);

			this.AuthedUser.UpdateFromMetadataModel(MM);
			this.AuthedUser.RegistrationStage = Models.UserModel.User.REGISTRATION_STAGE.RegistrationCompleted;
			this.AuthedUser.RegistrationCompleteDateTime = System.DateTime.UtcNow;

			await this.context.SaveChangesAsync();

			return RedirectToAction("Show", "User", new { Id = AuthedUser.Id });
		}
	}
}
