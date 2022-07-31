using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Website.Repository;
using Website.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Website.Middleware
{
	public class RegistrationStepsMiddleware
	{
		private RequestDelegate next;

		public RegistrationStepsMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		private enum ForwardingTo
		{
			LogoutPage,
			EnteringNamePage,
			EnteringMetadataPage,
			OtherPages
		}
		public async Task Invoke(HttpContext context)
		{
			var AuthedUser = (Models.UserModel.User?)context.Items[AuthenticationMiddleware.AuthedUsedObjectItemName];

			if (AuthedUser is null) // no user authed
			{
				await this.next.Invoke(context);
				return;
			}

			var act = context.GetRouteValue("Action") ?? string.Empty;
			var ctr = context.GetRouteValue("controller") ?? string.Empty;

			ForwardingTo CurrentDist = ForwardingTo.OtherPages;

			var UserCtrName = nameof(Website.Controllers.UserController).Replace("Controller", string.Empty);
			var LogoutActName = nameof(Website.Controllers.UserController.Logout);
			var EntNameActName = nameof(Website.Controllers.UserController.RegistrationStageEnteringName);
			var EntMetaActName = nameof(Website.Controllers.UserController.RegistrationStageEnteringMetadata);

			if (ctr.Equals(UserCtrName))
			{
				if (act.Equals(LogoutActName))
					CurrentDist = ForwardingTo.LogoutPage; // could return here, but returning just anywhere is not good
				else if (act.Equals(EntNameActName))
					CurrentDist = ForwardingTo.EnteringNamePage;
				else if (act.Equals(EntMetaActName))
					CurrentDist = ForwardingTo.EnteringMetadataPage;
			}

			if (CurrentDist == ForwardingTo.LogoutPage) // If trying to logout, let him
			{
				await this.next.Invoke(context);
				return;
			}

			ForwardingTo NeededDist = ForwardingTo.OtherPages;

			if (AuthedUser.RegistrationStage == Models.UserModel.User.REGISTRATION_STAGE.EnteringName)
				NeededDist = ForwardingTo.EnteringNamePage;
			else if (AuthedUser.RegistrationStage == Models.UserModel.User.REGISTRATION_STAGE.EnteringMetadata)
				NeededDist = ForwardingTo.EnteringMetadataPage;

			if (NeededDist == CurrentDist) // already heading needed page
			{
				await this.next.Invoke(context);
				return;
			}

			if (NeededDist == ForwardingTo.EnteringNamePage)
				context.Response.Redirect($"/{UserCtrName}/{EntNameActName}");
			else if (NeededDist == ForwardingTo.EnteringMetadataPage)
				context.Response.Redirect($"/{UserCtrName}/{EntMetaActName}");

			await this.next.Invoke(context);
			return;
		}
	}
}
