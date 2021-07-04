using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public void Add(T entity);
        public void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T? GetById(int id);
        T? Get(Expression<Func<T, bool>> where);
        T? GetNoTracking(Expression<Func<T, bool>> where);
        Task<T?> GetAsync(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IEnumerable<T> GetManyNoTracking(Expression<Func<T, bool>> where);
        IEnumerable<T> GetByPage(Expression<Func<T, bool>> where, int page = 0, int itemsPerPage = 10);
        Task<long> CountAsync(Expression<Func<T, bool>> where);
    }
}
