using AutoMapper;
using GymManagmentBLL.ViewModels.MemberViewModel;
using GymManagmentBLL.ViewModels.PlanViewModel;
using GymManagmentBLL.ViewModels.SessionViewModel;
using GymManagmentBLL.ViewModels.TrainerViewModel;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL
{
	public class MappingProfile :Profile
	{
		public MappingProfile()
		{
			MapMember();
			MapTrainer();
			MapSession();
			MapPlan();
		
			
		}
		private void MapTrainer()
		{
			CreateMap<Trainer, TrainerViewModel>()
		.ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => Enum.GetName(typeof(Specialities), src.Specialities)))
		.ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber}, {src.Address.Street}, {src.Address.City}"));

			CreateMap<Trainer, TrainerToUpdateViewModel>()
				.ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
				.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
				.ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

			CreateMap<TrainerToUpdateViewModel, Trainer>()
				.ForMember(dest => dest.Name, opt => opt.Ignore())
				.AfterMap((src, dest) =>
				{
					if (dest.Address == null) dest.Address = new Address(); // Prevent NullReference
					dest.Address.BuildingNumber = src.BuildingNumber;
					dest.Address.City = src.City;
					dest.Address.Street = src.Street;
					dest.UpdatedAt = DateTime.Now;
				});
		}
		private void MapSession()
		{
			CreateMap<CreateSessionViewModel, Session>();
			CreateMap<Session, SessionViewModel>()
						.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
						.ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
						.ForMember(dest => dest.AvailableSlot, opt => opt.Ignore()); // Will Be Calculated After Map
			CreateMap<UpdateSessionViewModel, Session>().ReverseMap();

			CreateMap<Trainer, TrainerSelectViewModel>();
			CreateMap<Category, CategorySelectViewModel>()
				.ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.CategoryName));



		}
		private void MapMember()
		{
			CreateMap<CreateMemberViewModel, Member>()
				  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
				  {
					  BuildingNumber = src.BuildingNumber,
					  City = src.City,
					  Street = src.Street
				  })).ForMember(dest => dest.healthRecord, opt => opt.MapFrom(src => src.HealthRecord));


			CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();
			CreateMap<Member, MemberViewModels>()
		   .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
			.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
			.ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

			CreateMap<Member, MemberToUpdateViewModel>()
			.ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
			.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
			.ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

			CreateMap<MemberToUpdateViewModel, Member>()
				.ForMember(dest => dest.Name, opt => opt.Ignore())
				.ForMember(dest => dest.phote, opt => opt.Ignore())
				.AfterMap((src, dest) =>
				{
					dest.Address.BuildingNumber = src.BuildingNumber;
					dest.Address.City = src.City;
					dest.Address.Street = src.Street;
					dest.UpdatedAt = DateTime.Now;
				});
		}
		private void MapPlan()
		{
			CreateMap<Plane, PlanViewModel>();
			CreateMap<Plane, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
			CreateMap<UpdatePlanViewModel, Plane>()
		   .ForMember(dest => dest.Name, opt => opt.Ignore())
		   .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

		}
	}
}
