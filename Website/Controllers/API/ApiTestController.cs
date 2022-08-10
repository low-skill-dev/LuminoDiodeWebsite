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
using Microsoft.AspNetCore.Http;
using Website.Repository;
using Microsoft.DotNet.MSIdentity.Shared;
using Website.Models.UserModel;
using Microsoft.AspNetCore.Authorization;

namespace Website.Controllers
{

	[AllowAnonymous]
	[Route("api/[controller]")]
	public sealed class ApiTestController : Controller
	{
		private readonly WebsiteContext ctx;
		public ApiTestController(WebsiteContext  ctx)
		{
			this.ctx = ctx;
		}


		[HttpGet("GetAllUsers")]
		public async Task<IActionResult> GetAllUsers()
		{
			try
			{
				return Json(await this.ctx.Users.ToListAsync());
			}
			catch
			{
				return StatusCode(StatusCodes.Status404NotFound);
			}
		}

		[HttpGet("GetUser/{id:int}")]
		public   async Task<IActionResult> GetUser(int id)
		{
			try
			{
				return Json(await this.ctx.Users.FindAsync(id));
			}
			catch
			{
				return StatusCode(StatusCodes.Status404NotFound);
			}
		}
		[HttpDelete("DeleteUser/{id:int}")]
		public  async Task<IActionResult> DeleteUser(int id)
		{
			try
			{
				var ent = await this.ctx.Users.FindAsync(id);
				if(ent is not null ) this.ctx.Users.Remove(ent);
				await this.ctx.SaveChangesAsync();

				return StatusCode(StatusCodes.Status200OK);
			}
			catch
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
		[HttpPatch("PatchUser")]
		// PowerShell usage example:
		// Invoke-RestMethod http://localhost:5000/api/ApiTest/PatchUser -Method PATCH -Body (@{Id=1;DisplayedName="AdmTest"}|ConvertTo-Json) -ContentType "application/json"
		public async Task<IActionResult> PatchUser([FromBody] User usr)
		{
			try
			{
				var ent = await this.ctx.Users.FindAsync(usr.Id);
				if (ent != null) {
					usr.EmailAdress = ent.EmailAdress; // cannot be modified cause email is an alternate key!!!
					this.ctx.Entry(ent).CurrentValues.SetValues(usr);
					await this.ctx.SaveChangesAsync();
					return StatusCode(StatusCodes.Status200OK);
				}
				else
				{
					return StatusCode(StatusCodes.Status404NotFound);
				}
			}
			catch(Exception ex)
			{
				var t = ex;
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
