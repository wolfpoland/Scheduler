using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Scheduler.Model;

namespace Scheduler.Data.Repositories
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        void Add(T entity);
        IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        void Commit();
        int Count();
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        T GetSingle(int id);
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Update(T entity);
    }
}