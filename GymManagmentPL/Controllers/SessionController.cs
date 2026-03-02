using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
	public class SessionController : Controller
	{
		private readonly ISessionService _sessionService;

		public SessionController(ISessionService sessionService)
		{
			_sessionService = sessionService;
		}
		public ActionResult Index()
		{
			var Sessions = _sessionService.GetAllSessions();
			return View(Sessions);
		}
		public ActionResult Details(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Invalid id ";
				return RedirectToAction(nameof(Index));
			}
			var Session=_sessionService.GetSessionById(id);
			if(Session is null)
			{
				TempData["ErrorMessage"] = "Session Not Found ";
				return RedirectToAction(nameof(Index));

			}
			return View(Session);

		}

		public ActionResult Create()
		{
			LoadDropDownCategory();
			LoadDropDownTrainer();
			return View();
		}
		[HttpPost]
		public ActionResult Create(CreateSessionViewModel createSession)
		{
			if (!ModelState.IsValid)
			{
				LoadDropDownCategory();
				LoadDropDownTrainer();
				return View(createSession);


			}
			var result= _sessionService.CreateSession(createSession);
			if (result)
			{
				TempData["SuccessMessage"] = "Session Creat Successfully";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				TempData["ErrorMessage"] = "Session Faild to Creat ";
				LoadDropDownCategory();
				LoadDropDownTrainer();
				return View(createSession);



			}
		}
		public ActionResult Edit(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Invalid id ";
				return RedirectToAction(nameof(Index));
			}
			var session= _sessionService.GetSessionToUpdate(id);
			if (session is null)
			{
				TempData["ErrorMessage"] = "Session not found";
				return RedirectToAction(nameof(Index));


			}
			LoadDropDownTrainer();
			return View(session);


		}
		[HttpPost]
		public ActionResult Edit([FromRoute]int id, UpdateSessionViewModel updateSession)
		{
			if (!ModelState.IsValid)
			{
				LoadDropDownTrainer();
				return View(updateSession);
			}
			var result = _sessionService.UpdateSession(updateSession,id);
			if (result)
			{
				TempData["SuccessMessage"] = "Session Updated Successfully";

			}
			else
			{
				TempData["ErrorMessage"] = "Session Faild to Update ";

			}
			return RedirectToAction(nameof(Index));

		}

		public ActionResult Delete(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Invalid id ";
				return RedirectToAction(nameof(Index));
			}
			var session= _sessionService.GetSessionById(id);
			if(session is null)
			{
				TempData["ErrorMessage"] = "Session not found";
				return RedirectToAction(nameof(Index));
			}
			ViewBag.SessionId = session.Id;
			return View();
		}
		[HttpPost]
		public ActionResult DeleteConfirmed(int id)
		{
			var result= _sessionService.RemoveSession(id);
			if (result)
			{
				TempData["SuccessMessage"] = "Session deleted Successfully";

			}
			else
			{
				TempData["ErrorMessage"] = "Session cant deleted";

			}
			return RedirectToAction(nameof(Index));
		}

		private void LoadDropDownCategory()
		{
			var category = _sessionService.GetCategoryForDropDown();
			ViewBag.Categories = new SelectList(category, "Id", "Name");


		}
		private void LoadDropDownTrainer()
		{

			var trainers = _sessionService.GetTrainerForDropDown();
			ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
		}

	}
}
