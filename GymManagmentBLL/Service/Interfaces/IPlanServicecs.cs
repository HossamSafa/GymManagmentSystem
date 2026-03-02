using GymManagmentBLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
	public interface IPlanServicecs
	{
		IEnumerable<PlanViewModel> GetAllPlane();
		PlanViewModel? GetPlanById(int planid);
		UpdatePlanViewModel? GetPlanToUpdate(int planid);
		bool UpdatePlan(int planid, UpdatePlanViewModel planToUpdate);

		bool ToggleStatus(int planid);
	}
}
