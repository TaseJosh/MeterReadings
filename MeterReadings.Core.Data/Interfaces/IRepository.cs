using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MeterReadings.Core.Data.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        T Update(T entity);
        T Get(int id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void SaveAll(IEnumerable<T> entity);
        void SaveChanges();
        void Delete(T entity);

    }
}
