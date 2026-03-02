using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repository.Classes
{
 public class UnitOfWork : IUnitOfWork
	{
		private readonly Dictionary<Type,object> _repositories = new ();

		private readonly GymDBContext _dBContext;

		public UnitOfWork(GymDBContext dBContext,ISessionRepository sessionRepository)
		{
			_dBContext = dBContext;
			this.sessionRepository = sessionRepository;
		}

		public ISessionRepository sessionRepository { get; }

		public IGenericRepositorycs<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
		{
			//TEntity == member
			var Entitytype= typeof(TEntity);
			if (_repositories.TryGetValue(Entitytype, out var repo))
				return (IGenericRepositorycs<TEntity>) repo;
			var Newrepo= new GenericRepository<TEntity>(_dBContext);
			_repositories[Entitytype] = Newrepo;
			return Newrepo;

		}

		public int SaveChange()
		{
			return _dBContext.SaveChanges();
		}
	}
}
