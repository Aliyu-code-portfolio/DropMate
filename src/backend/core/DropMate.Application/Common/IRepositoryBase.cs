using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.Common
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities); 
    }
}
