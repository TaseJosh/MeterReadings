using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MeterReadings.Core.Data.Interfaces;

namespace MeterReadings.Core.Data.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected MeterReadingDbContext Context;

        protected GenericRepository(MeterReadingDbContext context)
        {
            Context = context;
        }

        public virtual void Add(T entity)
        {
            Context.Add(entity);
        }

        public virtual IEnumerable<T> All()
        {
            return Context.Set<T>().ToList();
        }

        public virtual void Delete(T entity)
        {
            Context.Remove(entity);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public virtual T Get(int id)
        {
            return Context.Find<T>(id);
        }

        public virtual void SaveAll(IEnumerable<T> entity)
        {
            Context.Set<T>().AddRange(entity);
            Context.SaveChanges();
        }

        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            return Context.Update(entity).Entity;
        }
    }
}
