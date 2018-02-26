using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Watch.DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(int Id);
        T GetById(int Id);
        IQueryable<T> Get();
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, object>> includes);
        List<T> GetAll(out int count, Expression<Func<T, bool>> where, int? skip, int? take, Expression<Func<T, int>> orderBy, Expression<Func<T, object>> includes);
    }
}
