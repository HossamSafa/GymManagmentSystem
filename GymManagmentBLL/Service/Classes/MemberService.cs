using AutoMapper;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.Service.Interfaces.AttachmentService;
using GymManagmentBLL.ViewModels.MemberViewModel;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
	public class MemberService : IMemberService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IAttachmentService _attachmentService;

		public MemberService(IUnitOfWork unitOfWork, IMapper mapper,IAttachmentService attachmentService )
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_attachmentService = attachmentService;
		}

		public bool CreateMember(CreateMemberViewModel createMember)
		{
			try
			{
				if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone))
					return false;
				var photoName= _attachmentService.Upload("members",createMember.PhotoFile);
				if(string.IsNullOrEmpty(photoName)) return false;	
				var member= _mapper.Map<Member>(createMember);
				member.phote = photoName;
				_unitOfWork.GetRepository<Member>().Add(member);
				var Iscreated= _unitOfWork.SaveChange() > 0;
				if (!Iscreated)
				{
					_attachmentService.Delete(photoName, "members");
					return false;

				}
				else
				
				return Iscreated;
				

			}
			catch (Exception)
			{

				return false;
			}
		
		}

		public IEnumerable<MemberViewModels> GetAllMember()
		{
			var Members= _unitOfWork.GetRepository<Member>().GetAll();
			if (Members is null || !Members.Any()) return [];
			#region Way01
			//var MemberViewmodel=new List<MemberViewModels>();
			//foreach (var Member in Members)
			//{
			//	var memberviewmodel = new MemberViewModels()
			//	{
			//		Id = Member.Id,
			//		Name = Member.Name,
			//		Email = Member.Email,
			//		Phone = Member.Phone,
			//		phote = Member.phote,
			//		Gender = Member.Gender.ToString()

			//	};
			//	MemberViewmodel.Add(memberviewmodel);
			//} 
			#endregion
			var MemberViewmodel=_mapper.Map<IEnumerable<MemberViewModels>>(Members);
			return MemberViewmodel;
		}

		public MemberViewModels? GetMemberDetails(int Memberid)
		{
			var Member = _unitOfWork.GetRepository<Member>().GetById(Memberid);
			if (Member is null) return null;

			var viewmodel = _mapper.Map<MemberViewModels>(Member);
			//active Membership
			var ActiveMembership= _unitOfWork.GetRepository<MemberShip>().GetAll(X=>X.MemberID==Memberid &&X.Staues=="Active")
				.FirstOrDefault();

			if (ActiveMembership is not null)
			{
				viewmodel.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
				viewmodel.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
				var plane= _unitOfWork.GetRepository<Plane>().GetById(ActiveMembership.PlaneId);
				viewmodel.PlaneName = plane?.Name;
			}
			return viewmodel;
		}

		public HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid)
		{
			var Memberhealthrecord= _unitOfWork.GetRepository<HealthRecord>().GetById(Memberid);
			if (Memberhealthrecord is null) return null;
			return _mapper.Map<HealthRecordViewModel>(Memberhealthrecord);
		}

		public MemberToUpdateViewModel? GetMemberToUpdate(int Memberid)
		{
			var Member= _unitOfWork.GetRepository<Member>().GetById(Memberid);
			if (Member is null) return null;
			
			return _mapper.Map<MemberToUpdateViewModel>(Member);
		}

	

		public bool UpdateMemeber(int Memberid, MemberToUpdateViewModel memberToUpdate)
		{
	
			try
			{
				var phoneexist= _unitOfWork.GetRepository<Member>().GetAll(x=>x.Phone==memberToUpdate.Phone && x.Id != Memberid);
				var emailexist = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == memberToUpdate.Email && x.Id != Memberid);
				if(emailexist.Any() ||phoneexist.Any())
					return false;

				var MemberRepo = _unitOfWork.GetRepository<Member>();
				var Member=MemberRepo.GetById(Memberid);
				if (Member is null) return false;
				_mapper.Map(memberToUpdate, Member);
				MemberRepo.Update(Member);
				return _unitOfWork.SaveChange() > 0;

			}
			catch
			{
				return false;

			}
		}

		public bool RemoveMember(int Memberid)
		{
			var MemberRepo= _unitOfWork.GetRepository<Member>();
			var member= MemberRepo.GetById(Memberid);
			if (member is null) return false;
			var HasactiveMembersession = _unitOfWork.GetRepository<MemberSession>()
				.GetAll(x => x.MemberId == Memberid && x.Session.StartDate > DateTime.Now).Any();
			if (HasactiveMembersession) return false;
			var MmbershipRepo = _unitOfWork.GetRepository<MemberShip>();
			var Membership = MmbershipRepo.GetAll(x => x.MemberID == Memberid);
			try
			{
				if (Membership.Any())
				{
					foreach (var membership in Membership)
					{
						MmbershipRepo.Delete(membership);
					}
				}

			 MemberRepo.Delete(member);
			var IsDeleted= _unitOfWork.SaveChange() > 0;
				if (IsDeleted)
					_attachmentService.Delete(member.phote, "members");
				return IsDeleted;

			}
			catch
			{
				return false;
			}

		}

		#region Helper Method
		private bool IsEmailExist(string email)
		{
			return _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == email).Any();
		}
		private bool IsPhoneExist(string phone)
		{
			return _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == phone).Any();
		}
		#endregion
	}
}
