using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
	public class PlanController : Controller
	{
		private readonly IPlanServicecs _planServicecs;

		public PlanController(IPlanServicecs planServicecs)
		{
			_planServicecs = planServicecs;
		}
		public IActionResult Index()
		{
			var plans = _planServicecs.GetAllPlane();
			return View(plans);
		}
		public ActionResult Details(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Invalid plan id";
				return RedirectToAction(nameof(Index));
			}
			var plan= _planServicecs.GetPlanById(id);
			if(plan is null)
			{
				TempData["ErrorMessage"] = "plan not found";
				return RedirectToAction(nameof(Index));


			}
			return View(plan);


		}

		public ActionResult Edit(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Invalid plan id";
				return RedirectToAction(nameof(Index));
			}
			var plan= _planServicecs.GetPlanToUpdate(id);
			if (plan is null)
			{
				TempData["ErrorMessage"] = "plan  cant be updated";
				return RedirectToAction(nameof(Index));


			}
			return View(plan);


		}

		[HttpPost]
		public ActionResult Edit([FromRoute]int id, UpdatePlanViewModel updatePlan)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("WrongData", "check data validation");
				return View(updatePlan);
			}
			var result= _planServicecs.UpdatePlan(id, updatePlan);
			if (result)
			{
				TempData["SuccessMessage"] = "Plan updated successfully";
			}
			else
			{
				TempData["ErrorMessage"] = "plan faild to update";
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public ActionResult Activate(int id)
		{
			var result= _planServicecs.ToggleStatus(id);
			if (result)
			{
				TempData["SuccessMessage"] = "Plan status change";


			}
			else
			{
				TempData["ErrorMessage"] = "Faild to change Plan status ";

			}
			return RedirectToAction(nameof(Index));

		}
	}
}
