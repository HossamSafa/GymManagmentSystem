using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repository.Classes
{
	public class SessionRepository : GenericRepository<Session>, ISessionRepository
	{
		private readonly GymDBContext _dBContext;

		public SessionRepository(GymDBContext dBContext) : base(dBContext)
		{
			_dBContext = dBContext;
		}
		public IEnumerable<Session> GetAllSesiionWithTrainerAndCategory()
		{
			return _dBContext.Sessions.Include(x => x.SessionTrainer).Include(x => x.SessionCategory).ToList();
		}

		public int GetCountofBookedSlot(int sessionid)
		{
			return _dBContext.MembersSessions.Where(x=>x.SessionId == sessionid).Count();
		}

		public Session? GetSessionWithTrainerandCategory(int sessionid)
		{
			return _dBContext.Sessions.Include(X=>X.SessionTrainer).Include(X=>X.SessionCategory).
				FirstOrDefault(x=>x.Id== sessionid);
		}
	}
}
