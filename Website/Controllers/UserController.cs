using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Website.Models.ViewModels;
using Website.Services;
using System.Linq;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace Website.Controllers
{
	public sealed class UserController : AMyController
	{
		private readonly Website.Services.PasswordsService passwordsService;
		private readonly Website.Services.AuthTockenService authTockenService;
		public UserController(IServiceScopeFactory Services, Website.Services.RecentDocumentsBackgroundService documentsBackgroundService, SessionManager SM)
			: base(Services)
		{
			var sp = Services.CreateScope().ServiceProvider;
			this.passwordsService = sp.GetRequiredService<PasswordsService>();
			this.authTockenService = sp.GetRequiredService<AuthTockenService>();
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
			else return this.View(found);
		}

		[HttpGet]
		public IActionResult Login()
		{
			var RequestDebug = this.Request;

			if (base.AuthedUser != null) return new StatusCodeResult(409); // 409 "Conflict", already signed in
			else return this.View();
		}
		[HttpPost]
		public async Task<ActionResult> Login([Bind] Website.Models.Auth.LoginInfo LI)
		{
			var RequestDebug = this.Request;

			if (!this.ModelState.IsValid)
				return this.View(LI);

			var found = await this.context.Users.FirstOrDefaultAsync(x => x.EmailAdress == LI.EmailPlainText);

			if (found == null)
			{
				base.AddAlertToPageTop(new Alert("User not found", Alert.ALERT_TYPE.Danger));
				return this.View(LI);
			}
			if (found.AuthHashedPassword is null || found.AuthPasswordSalt is null)
			{
				return new StatusCodeResult(500); // should never be returned in prod
			}

			if (this.passwordsService.ConfirmPassword(LI.PasswordPlainText, found.AuthHashedPassword, found.AuthPasswordSalt))
			{
				base.SM.CreateSession(found.Id, out var CreatedSessId);
				this.Response.Cookies.Append(SessionManager.SessionIdCoockieName, CreatedSessId);
				return this.RedirectToAction("Show", "User", new { id = found.Id });
			}
			else
			{
				base.AddAlertToPageTop(new Alert("Wrong email or password", Alert.ALERT_TYPE.Danger));
				return this.View();
			}
		}

		[HttpGet]
		public ActionResult Logout()
		{
			this.Response.Cookies.Delete(SessionManager.SessionIdCoockieName);
			return this.RedirectToAction("Summary", "Home");
		}

		[HttpGet]
		public ActionResult Register()
		{
			if (base.AuthedUser != null) return new StatusCodeResult(409); // 409 "Conflict", already signed in
			else return this.View();
		}
		private const string DefaultProfileImage_230x230 = "iVBORw0KGgoAAAANSUhEUgAAAOYAAADmCAYAAADBavm7AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAUWaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA2LjAtYzAwMiA3OS4xNjQ0NjAsIDIwMjAvMDUvMTItMTY6MDQ6MTcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCAyMS4yIChXaW5kb3dzKSIgeG1wOkNyZWF0ZURhdGU9IjIwMjItMDQtMjBUMTc6MjE6MjYrMDM6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDIyLTA0LTIwVDE3OjIzOjM5KzAzOjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIyLTA0LTIwVDE3OjIzOjM5KzAzOjAwIiBkYzpmb3JtYXQ9ImltYWdlL3BuZyIgcGhvdG9zaG9wOkNvbG9yTW9kZT0iMiIgcGhvdG9zaG9wOklDQ1Byb2ZpbGU9InNSR0IgSUVDNjE5NjYtMi4xIiB4bXBNTTpJbnN0YW5j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";
		[HttpPost]
		public async Task<IActionResult> Register(Website.Models.Auth.LoginInfo LI)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(LI);
			}

			var EmailPlainText = LI.EmailPlainText;
			var PasswordPlainText = LI.PasswordPlainText;

			Website.Models.UserModel.User? found;
			found = await this.context.Users.FirstOrDefaultAsync(x => x.EmailAdress.Equals(EmailPlainText));

			if (found != null)
			{
				base.AddAlertToPageTop(new Alert("This email is already occupied", Alert.ALERT_TYPE.Danger));
				var t = TempData;
				return this.View();
			}

			else
			{
				var hashedpass = this.passwordsService.HashPassword(PasswordPlainText, out var Salt);

				var UserToAdd = new Models.UserModel.User
				{
					EmailAdress = EmailPlainText,
					AuthHashedPassword = hashedpass,
					AuthPasswordSalt = Salt,
					DisplayedName = "New User",
					String64_ProfileImage = DefaultProfileImage_230x230
				};
				this.context.Users.Add(UserToAdd);
				await this.context.SaveChangesAsync();
				UserToAdd.DisplayedName = "user" + UserToAdd.Id;
				await this.context.SaveChangesAsync();
				base.AddAlertToPageTop(new Alert("Account created. Log in.", Alert.ALERT_TYPE.Info));
				var t = TempData;
				return this.RedirectToAction("Login");
			}
		}

		[HttpGet]
		public IActionResult RegistrationStageEnteringName()
		{
			if (this.AuthedUser == null)
				return new StatusCodeResult(401); // 401 Unauthorized
			if (this.AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.EnteringName)
				return new StatusCodeResult(422); // 422 Unprocessable Entity

			return this.View();
		}
		[HttpPost]
		public async Task<IActionResult> RegistrationStageEnteringName(Website.Models.UserModel.NameModel NM)
		{
			if (this.AuthedUser == null)
				return new StatusCodeResult(401); // 401 Unauthorized
			if (this.AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.EnteringName)
				return new StatusCodeResult(422); // 422 Unprocessable Entity


			if (!this.ModelState.IsValid)
				return this.View(NM);

			this.AuthedUser.UpdateFromNameModel(NM);
			this.AuthedUser.RegistrationStage = Models.UserModel.User.REGISTRATION_STAGE.EnteringMetadata;
			await this.context.SaveChangesAsync();

			return this.RedirectToAction("RegistrationStageEnteringMetadata", "User");
		}
		[HttpGet]
		public IActionResult RegistrationStageEnteringMetadata()
		{
			if (this.AuthedUser == null)
				return new StatusCodeResult(401); // 401 Unauthorized
			if (this.AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.EnteringMetadata)
				return new StatusCodeResult(422); // 422 Unprocessable Entity

			return this.View();
		}
		[HttpPost]
		public async Task<IActionResult> RegistrationStageEnteringMetadata(Website.Models.UserModel.MetadataModel MM)
		{
			if (this.AuthedUser == null)
				return new StatusCodeResult(401); // 401 Unauthorized
			if (this.AuthedUser.RegistrationStage != Models.UserModel.User.REGISTRATION_STAGE.EnteringMetadata)
				return new StatusCodeResult(422); // 422 Unprocessable Entity

			MM.TrimAllFields();
			if (!this.ModelState.IsValid)
				return this.View(MM);

			this.AuthedUser.UpdateFromMetadataModel(MM);
			this.AuthedUser.RegistrationStage = Models.UserModel.User.REGISTRATION_STAGE.RegistrationCompleted;
			this.AuthedUser.RegistrationCompleteDateTime = System.DateTime.UtcNow;

			await this.context.SaveChangesAsync();

			return this.RedirectToAction("Show", "User", new { Id = this.AuthedUser.Id });
		}

		[HttpGet]
		public IActionResult NewAuthLogin()
		{
			return View();
		}

		public const string PasswordSaltString64 = "PasswordSaltString64";
		public const string AuthTockenName = "AuthTocken";
		public const string AuthHashKeyString64 = "AuthHashKeyString64";
		public const string LoginRouteValueName = "Login";
		public const string PasswordHashedByClientFormName = "hashedPassword64";
		public const string CurrentLoginCoockieName = "crrlgn";
		[HttpPost]
		public async Task<IActionResult> NewAuthLogin(Website.Models.Auth.LoginOnly LO)
		{
			if (!ModelState.IsValid)
				return View(LO);

			Response.Cookies.Append(CurrentLoginCoockieName, LO.EmailPlainText);
			return RedirectToAction(nameof(NewAuthPassword));
		}

		[HttpGet]
		public async Task<IActionResult> NewAuthPassword()
		{
			var passedLogin = Request.Cookies[CurrentLoginCoockieName];
			if (passedLogin is null)
			{
				return new StatusCodeResult(400); // trying to enter password without entering login
			}

			Website.Models.Auth.LoginOnly LO = new() { EmailPlainText = passedLogin };

			if (!TryValidateModel(LO))
			{
				AddAlertToPageTop(new("Bad email format", Alert.ALERT_COLOR.Red));
				return this.RedirectToAction(nameof(NewAuthLogin));
			}

			var found = await this.context.Users.FirstOrDefaultAsync(x => x.EmailAdress!.Equals(LO.EmailPlainText));

			if (found is null)
			{
				base.AddAlertToPageTop(new Alert("User not found", Alert.ALERT_TYPE.Danger));
				return this.RedirectToAction(nameof(NewAuthLogin));
			}
			if (found.AuthHashedPassword is null || found.AuthPasswordSalt is null)
			{
				return new StatusCodeResult(500); // should never be returned in prod
			}

			this.authTockenService.CreateTocken(found.Id, out var AuthTocken, out var AuthHashKey);

			this.Response.Cookies.Append(PasswordSaltString64, Base64UrlEncoder.Encode(found.AuthPasswordSalt));
			this.Response.Cookies.Append(AuthTockenName, AuthTocken);
			this.Response.Cookies.Append(AuthHashKeyString64, Base64UrlEncoder.Encode(AuthHashKey));

			return View();
		}

		//private static string Base64ToUrl(string s)=> s.Replace()
		[HttpPost]
		public async Task<IActionResult> NewAuthPassword(string hashedPassword64)
		{
			if (string.IsNullOrEmpty(hashedPassword64))
				return new StatusCodeResult(400);// no password was sent to server

			byte[] PasswordHashByClient = null!;
			try { PasswordHashByClient = Base64UrlEncoder.DecodeBytes(hashedPassword64); }
			catch { return new StatusCodeResult(422); }// incorrect base64 was sent as password

			var AuthTockenId = this.Request.Cookies[AuthTockenName];
			if (string.IsNullOrEmpty(AuthTockenId))
			{
				AddAlertToPageTop(new("Unable to load auth tocken. Please retry.", Alert.ALERT_COLOR.Red));
				return this.RedirectToAction(nameof(NewAuthLogin)); // no auth tocken
			}

			if (!this.authTockenService.TryGetTocken(AuthTockenId, out var Tocken))
			{
				AddAlertToPageTop(new("Auth tocken is outdated. Please retry.", Alert.ALERT_COLOR.Red));
				return this.RedirectToAction(nameof(NewAuthLogin)); // tocken does not exists on the server
			}

			var FoundUser = await this.context.Users.FindAsync(Tocken.UserId);
			if (FoundUser is null)
			{
				base.AddAlertToPageTop(new Alert("User not found", Alert.ALERT_TYPE.Danger));
				return this.RedirectToAction(nameof(NewAuthLogin));
			}
			if (FoundUser.AuthHashedPassword is null || FoundUser.AuthPasswordSalt is null)
			{
				return new StatusCodeResult(500); // should never be returned in prod
			}

			if (!this.authTockenService.ConfirmPasswordAndInvalidateTocken(AuthTockenId, PasswordHashByClient, FoundUser.AuthHashedPassword, out var dummy))
			{
				AddAlertToPageTop(new("Wrong password", Alert.ALERT_COLOR.Red));
				return RedirectToAction(nameof(NewAuthPassword));
			}

			base.SM.CreateSession(FoundUser.Id, out var CreatedSessId);
			this.Response.Cookies.Append(SessionManager.SessionIdCoockieName, CreatedSessId);

			this.Response.Cookies.Delete(PasswordSaltString64);
			this.Response.Cookies.Delete(AuthTockenName);
			this.Response.Cookies.Delete(AuthHashKeyString64);

			return this.RedirectToAction("Show", "User", new { id = FoundUser.Id });
		}

	}
}
