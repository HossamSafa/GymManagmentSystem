using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
	[Authorize(Roles = "SuperAdmin")]

	public class MemberController : Controller
	{
		private readonly IMemberService _memberService;

		public MemberController(IMemberService memberService)
		{
			_memberService = memberService;
		}
		//get all member
		public ActionResult Index()
		{
			var member= _memberService.GetAllMember();
			return View(member);
		}

		public ActionResult MemberDetails(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Id of member cant be 0 or negative";
				return RedirectToAction(nameof(Index));
			}
			var member= _memberService.GetMemberDetails(id);
			if (member is null)
			{
				TempData["ErrorMessage"] = "Member not found";

				return RedirectToAction(nameof(Index));
			}
			return View(member);


		}

		public ActionResult HealthRecordDetails(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Id of member cant be 0 or negative";

				return RedirectToAction(nameof(Index));
			}
			var healthmember= _memberService.GetMemberHealthRecordDetails(id);
			if (healthmember is null)
			{
				TempData["ErrorMessage"] = "Member not found";

				return RedirectToAction(nameof(Index));
			}
			return View(healthmember);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateMember(CreateMemberViewModel createMember)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("DataMissed", "check data and Missing Field");
				return View(nameof(Create), createMember);
			}
			bool result= _memberService.CreateMember(createMember);

			if (result)
			{
				TempData["SuccessMessage"] = "Member created succesfuly";
			}
			else
			{
				TempData["ErrorMessage"] = "Member faild to create";
			}
			return RedirectToAction (nameof(Index));	
		}

		public ActionResult MemberEdit(int id)
		{
			if(id <= 0)
			{
				TempData["ErrorMessage"] = "Id of member cant be 0 or negative";
				return RedirectToAction(nameof(Index));

			}
			var Member= _memberService.GetMemberToUpdate(id);
			if (Member is null)
			{
				TempData["ErrorMessage"] = "Member not found";

				return RedirectToAction(nameof(Index));

			}
			return View(Member);

		}
		[HttpPost]
		public ActionResult MemberEdit([FromRoute]int id , MemberToUpdateViewModel memberToUpdate)
		{
			if (!ModelState.IsValid) 
				return View(memberToUpdate);
			var result= _memberService.UpdateMemeber(id, memberToUpdate);
			if (result)
			{
				TempData["SuccessMessage"] = "Member Update succesfuly";
			}
			else
			{
				TempData["ErrorMessage"] = "Member faild to Update";
			}
			return RedirectToAction(nameof(Index));

		}

		public ActionResult Delete(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Id of member cant be 0 or negative";
				return RedirectToAction(nameof(Index));

			}
			var member= _memberService.GetMemberDetails(id);
			if (member is null)
			{
				TempData["ErrorMessage"] = "Member not found";

				return RedirectToAction(nameof(Index));

			}
			ViewBag.MemberId = id;
			return View();
		}
		[HttpPost]
		public ActionResult DeleteConfim([FromForm]int id)
		{
			var result = _memberService.RemoveMember(id);
			if (result)
			{
				TempData["SuccessMessage"] = "Member delete succesfuly";
			}
			else
			{
				TempData["ErrorMessage"] = "Member faild to delete";
			}
			return RedirectToAction(nameof(Index));


		}
	}
}
