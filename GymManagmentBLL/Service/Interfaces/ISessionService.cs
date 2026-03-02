using GymManagmentBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
	public interface ISessionService
	{
		IEnumerable<SessionViewModel> GetAllSessions();

		SessionViewModel? GetSessionById(int sessionid);

		bool CreateSession(CreateSessionViewModel createSession);

		UpdateSessionViewModel? GetSessionToUpdate(int sessionid);

		bool UpdateSession(UpdateSessionViewModel updateSession,int sessionid);

		bool RemoveSession(int sessionid);

		IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown();
		IEnumerable<CategorySelectViewModel> GetCategoryForDropDown();

	}
}
