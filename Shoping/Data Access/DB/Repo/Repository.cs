using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Repo
{
    public class Repository : IRepository
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            var _dbSet = _context.Set<TEntity>();
            _dbSet.Add(entity);
        }

        public Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : class
        {
            var _dbSet = _context.Set<TEntity>();
            return _dbSet.Where(condition).ToListAsync();
        }

        public Task<TEntity?> GetOneAsync<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : class
        {
            var _dbSet = _context.Set<TEntity>();
            return _dbSet.FirstOrDefaultAsync(condition);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            var _dbSet = _context.Set<TEntity>();
            _dbSet.Update(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            var _dbSet = _context.Set<TEntity>();
            _dbSet.Remove(entity);
        }

        public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            var _dbSet = _context.Set<TEntity>();
            _dbSet.RemoveRange(entities);
        }
    }
}
