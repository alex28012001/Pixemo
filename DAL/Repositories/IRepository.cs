using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IRepository<T> where T:class
    {
        void Create(T item);
        void RemoveByID(int id);
        Task RemoveByIDAsync(int id);
        void Remove(T item);
        int CountWithExpressionTree(Expression<Func<T, Boolean>> predicate);
        void Update(T item);
        T GetByID(int id);
        Task<T> GetByIDAsync(int id);
        bool Any();
        bool Any(Func<T, Boolean> predicate);
        bool AnyWithExpressionTree(Expression<Func<T, Boolean>> predicate);
        IEnumerable<T> Find(Func<T,Boolean> predicate);
        IEnumerable<T> FindWithExpressionTree(Expression<Func<T, Boolean>> predicate);
        IEnumerable<T> GetAll();
    }
}
