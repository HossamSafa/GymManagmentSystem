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
	public class GenericRepository<TEntity> : IGenericRepositorycs<TEntity> where TEntity : BaseEntity, new()
	{
		private readonly GymDBContext _dBContext;

		public GenericRepository(GymDBContext dBContext)
		{
			_dBContext = dBContext;
		}
		public void Add(TEntity entity)=> _dBContext.Set<TEntity>().Add(entity);


		public void Delete(TEntity entity)=> _dBContext.Set<TEntity>().Remove(entity);




		public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
		{
			if (condition is null)
				return _dBContext.Set<TEntity>().AsNoTracking().ToList();
			else 
				return _dBContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
		}

		public TEntity? GetById(int id) => _dBContext.Set<TEntity>().Find(id);


		public void Update(TEntity entity)=> _dBContext.Set<TEntity>().Update(entity);

	}
}
