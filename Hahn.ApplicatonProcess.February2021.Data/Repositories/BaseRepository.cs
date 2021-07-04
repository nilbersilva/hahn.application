using Hahn.ApplicatonProcess.February2021.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        private DbSet<T> DbSet => DbContext.Set<T>();
        protected DataContext DbContext { get; set; }

        protected BaseRepository(DataContext context)
        {
            DbContext = context;
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = DbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                DbSet.Remove(obj);
        }

        public T? GetById(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where);
        }

        public IEnumerable<T> GetManyNoTracking(Expression<Func<T, bool>> where)
        {
            return DbSet.AsNoTracking().Where(where);
        }

        public T? Get(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault<T>();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> where)
        {
            return await DbSet.Where(where).FirstOrDefaultAsync<T>();
        }

        public T? GetNoTracking(Expression<Func<T, bool>> where)
        {
            return DbSet.AsNoTracking().Where(where).FirstOrDefault<T>();
        }

        public IEnumerable<T> GetByPage(Expression<Func<T, bool>> where, int page = 0, int itemsPerPage = 10)
        {
            if (where != null)
            {
                return DbSet.Where(where).Skip(page * itemsPerPage).Take(itemsPerPage).ToList();
            }
            else
            {
                return DbSet.Skip(page * itemsPerPage).Take(itemsPerPage).ToList();
            }            
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            if (where != null)
            {
                return await DbSet.Where(where).CountAsync();
            }
            else
            {
                return await DbSet.CountAsync();
            }
        }
    }
}
