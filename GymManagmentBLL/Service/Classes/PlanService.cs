using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.PlanViewModel;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
	public class PlanService : IPlanServicecs
	{
		private readonly IUnitOfWork _unitOfWork;

		public PlanService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IEnumerable<PlanViewModel> GetAllPlane()
		{
			var planes = _unitOfWork.GetRepository<Plane>().GetAll();
			if (planes is null || !planes.Any()) return [];

			return planes.Select(p => new PlanViewModel
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				Description=p.Description,
				DurationDayes = p.DurationDays,
				IsActive = p.IsActive

			});
		}

		public PlanViewModel? GetPlanById(int planid)
		{
			var plan= _unitOfWork.GetRepository<Plane>().GetById(planid);
			if (plan is null) return null;
			return new PlanViewModel
			{
				Id= plan.Id,
				Name = plan.Name,
				Price = plan.Price,
				Description = plan.Description,
				DurationDayes = plan.DurationDays,
				IsActive = plan.IsActive

			};
		}

		public UpdatePlanViewModel? GetPlanToUpdate(int planid)
		{
			var  plan= _unitOfWork.GetRepository<Plane>().GetById(planid);
			if (plan is null|| HasActivemembership(planid)) return null;
			return new UpdatePlanViewModel()
			{
				Description = plan.Description,
				DurationDays = plan.DurationDays,
				PlanName = plan.Name,
				Price = plan.Price
			};
		}



		public bool UpdatePlan(int planid, UpdatePlanViewModel planToUpdate)
		{
			var plan = _unitOfWork.GetRepository<Plane>().GetById(planid);
			if (plan is null || HasActivemembership(planid)) return false;

			(plan.Description, plan.Price, plan.DurationDays, plan.Name) =
				(planToUpdate.Description, planToUpdate.Price,planToUpdate.DurationDays,planToUpdate.PlanName);
			_unitOfWork.GetRepository<Plane>().Update(plan);
			return _unitOfWork.SaveChange() > 0;

		}

		public bool ToggleStatus(int planid)
		{
			var repo= _unitOfWork.GetRepository<Plane>();
			var plane= repo.GetById(planid);
			if(plane is null || HasActivemembership(planid)) return false;
			plane.IsActive= plane.IsActive == true ? false : true;
			plane.UpdatedAt = DateTime.Now;
			try
			{
				repo.Update(plane);
				return _unitOfWork.SaveChange() > 0;

			}
			catch
			{
				return false;
			}
		}
		#region Helper
		private bool HasActivemembership(int planid)
		{
			var activemembership = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.PlaneId == planid && x.Staues == "Active");
			return activemembership.Any();
		}
		#endregion
	}
}
