using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repository.Interfaces
{
	public interface IUnitOfWork
	{
		public ISessionRepository sessionRepository { get; }

		IGenericRepositorycs<TEntity>GetRepository<TEntity>() where TEntity : BaseEntity, new();


		int SaveChange();
	}
}
