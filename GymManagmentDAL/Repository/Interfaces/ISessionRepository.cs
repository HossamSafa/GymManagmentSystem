using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repository.Interfaces
{
	public interface ISessionRepository : IGenericRepositorycs<Session>
	{
		IEnumerable<Session> GetAllSesiionWithTrainerAndCategory();

		int GetCountofBookedSlot(int sessionid);

		Session? GetSessionWithTrainerandCategory(int sessionid);
	}
}
