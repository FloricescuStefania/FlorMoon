using System.Linq.Expressions;
using System;
using System.Linq;

namespace FlorMoon.Repository.Interfaces
{
        public interface IRepositoryBase<T>
        {
            IQueryable<T> FindAll();
            IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
            void Create(T entity);
            void Update(T entity);
            void Delete(T entity);
        }
}
