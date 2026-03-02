using AutoMapper;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.SessionViewModel;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
	public class SessionService : ISessionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public bool CreateSession(CreateSessionViewModel createSession)
		{
			try
			{   //check if trainer exist
				if (!ISTrainerExist(createSession.TrainerId)) return false;
				//check if category exist
				if (!ISCategoryExist(createSession.CategoryId)) return false;

				//check if start date before enddate
				if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate)) return false;
				if (createSession.Capacty > 25 || createSession.Capacty < 0) return false;
				var seesionEntity = _mapper.Map<Session>(createSession);
				_unitOfWork.GetRepository<Session>().Add(seesionEntity);
				return _unitOfWork.SaveChange() > 0;

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Create sessionFaild {ex}");
				return false;
			}


		}

		public IEnumerable<SessionViewModel> GetAllSessions()
		{
			var sessions= _unitOfWork.sessionRepository.GetAllSesiionWithTrainerAndCategory();
			if (!sessions.Any()) return [];
		
			var MappedSession=_mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

			foreach (var Session in MappedSession)
				Session.AvailableSlot = Session.Capacty - _unitOfWork.sessionRepository.GetCountofBookedSlot(Session.Id);
			return MappedSession;
				
					

		}
		public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown()
		{
			var trainer = _unitOfWork.GetRepository<Trainer>().GetAll();
			return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainer);
		}

		public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
		{
			var category=_unitOfWork.GetRepository<Category>().GetAll();
			return _mapper.Map <IEnumerable<CategorySelectViewModel>>(category);
		}

		public SessionViewModel? GetSessionById(int sessionid)
		{
			var Session=_unitOfWork.sessionRepository.GetSessionWithTrainerandCategory(sessionid);
			if(Session is null)	return null;
			var mappedSession=_mapper.Map<Session,SessionViewModel>(Session);
			mappedSession.AvailableSlot=mappedSession.Capacty- _unitOfWork.sessionRepository.GetCountofBookedSlot(sessionid);
			return mappedSession;

	
		}


		public UpdateSessionViewModel? GetSessionToUpdate(int sessionid)
		{
			var Session=_unitOfWork.sessionRepository.GetById(sessionid);
			if(!IsSessionAvailbletoupdate(Session!)) return null;

			return _mapper.Map<UpdateSessionViewModel>(Session);
		}

		public bool UpdateSession(UpdateSessionViewModel updateSession, int sessionid)
		{
			try
			{
				var session = _unitOfWork.sessionRepository.GetById(sessionid);
				if(!IsSessionAvailbletoupdate(session!)) return false;
				if(!ISTrainerExist(updateSession.TrainerId)) return false;
				if (!IsDateTimeValid(updateSession.StartDate, updateSession.EndDate)) return false;
				_mapper.Map(updateSession, session);
				session!.UpdatedAt=DateTime.Now;
				_unitOfWork.sessionRepository.Update(session);
				return _unitOfWork.SaveChange() > 0;


			}
			catch (Exception ex)
			{
				Console.WriteLine($"Update Session Faild {ex}");
				return false;
			}
		}

		public bool RemoveSession(int sessionid)
		{
			try
			{
				var session = _unitOfWork.sessionRepository.GetById(sessionid);
				if(!ISSessionavialbleforRemoving(session!)) return false;
				_unitOfWork.sessionRepository.Delete(session!);
				return _unitOfWork.SaveChange() > 0; 

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Remove session faild {ex}");
				return false;
			}
		}
		#region Helper
		private bool ISTrainerExist(int trainerid)
		{
			return _unitOfWork.GetRepository<Trainer>().GetById(trainerid) is not null;

		}
		private bool ISCategoryExist(int categoryid)
		{
			return _unitOfWork.GetRepository<Category>().GetById(categoryid) is not null;
		}
		private bool IsDateTimeValid(DateTime startdate, DateTime enddate)
		{
			return startdate < enddate;
		}

		private bool IsSessionAvailbletoupdate(Session session)
		{
			if(session is null) return false;
			//if session complete 
			if(session.EndDate<DateTime.Now) return false;
			//if session started
			if(session.StartDate<=DateTime.Now) return false;
			//if has active booking
			var hasactivebooking = _unitOfWork.sessionRepository.GetCountofBookedSlot(session.Id) > 0;
			if(hasactivebooking) return false;

			return true;

		}

		private bool ISSessionavialbleforRemoving(Session session)
		{
			if(session is null) return false;
			//if session started
			if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

			//if session is upcoming
			if (session.StartDate > DateTime.Now) return false;

			//if session have active booking
			var hasactivebooking = _unitOfWork.sessionRepository.GetCountofBookedSlot(session.Id) > 0;
			if(hasactivebooking) return false;
			return true;


		}





		#endregion
	}
}
