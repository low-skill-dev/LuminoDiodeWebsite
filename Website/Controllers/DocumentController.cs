using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
	public class DocumentController : Controller
	{
		[HttpGet]
		public ViewResult Summary()
		{
			return View();
		}

		[HttpGet]
		public ViewResult Show(int ProjectId)
		{
			return View();
		}

		#region Create
		[HttpGet]
		public ViewResult Create()
		{
			return View();
		}

		[HttpPost]
		public StatusCodeResult Create(object DocumentPassedForCreation/*tobedefinied*/)
		{
			/* HTTP Status 202 indicates that the request 
			 * has been accepted for processing, 
			 * but the processing has not been completed.
			 */
			return new StatusCodeResult(202);
		}
		#endregion

		#region Edit
		[HttpGet]
		public ViewResult Edit(int ProjectId)
		{
			return View();
		}

		[HttpPut]
		public StatusCodeResult Edit(int ProjectId, object DocumentToBePosted)
		{
			return new StatusCodeResult(202);
		}
		#endregion

		#region Delete
		[HttpGet]
		public ViewResult Delete(int ProjectId)
		{
			return View();
		}
		[HttpDelete]
		public StatusCodeResult Delete(int ProjectId, int UserPerformsActionId)
		{
			return new StatusCodeResult(202);
		}
		#endregion

	}
}
