using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Repo
{
    interface IRepository
    {
        void Add<TEntity>(TEntity entity) where TEntity : class;
        Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : class;
        Task<TEntity?> GetOneAsync<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    }
}
