using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.AnalyticsViewModel;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
	public class AnalyticsService : IAnalyticsService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AnalyticsService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public AnalyticsViewModel GetAnalyticsData()
		{
			var session= _unitOfWork.GetRepository<Session>().GetAll();

			return new AnalyticsViewModel
			{
				ActiveMember = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.Staues == "Active").Count(),
				TotalMember = _unitOfWork.GetRepository<Member>().GetAll().Count(),
				TotalTrainer = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
				UpcomingSession = session.Where(x => x.StartDate > DateTime.Now).Count(),
				OnGoingSession=session.Where(x=>x.StartDate<=DateTime.Now &&x.EndDate >= DateTime.Now).Count(),
				CompleteSession=session.Where(x=>x.EndDate < DateTime.Now).Count(),
			};

		}
	}
}
