using DropMate2.Application.Common;
using DropMate2.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DropMate2.Persistence.Common
{
    public abstract class RepositoryBase<T>:IRepositoryBase<T> where T : class, IEntityBase
    {
        public DbSet<T> _dbSet;

        public RepositoryBase(RepositoryContext repository)
        {
            _dbSet = repository.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }

        public void PermanentDelete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void PermanentDeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ? _dbSet.AsNoTracking() :
                _dbSet;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges)
        {
            return !trackChanges ? _dbSet.Where(condition).AsNoTracking() :
                _dbSet.Where(condition);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
