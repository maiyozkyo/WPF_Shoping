using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Repo
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        // Add a new entity into database
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        // Return a list of entity that match the condition
        public IQueryable<TEntity> GetAsync(Expression<Func<TEntity, bool>> condition)
        {
            return _dbSet.Where(condition);
        }
        // Return the first element of a list of entity that match the condition
        public Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> condition)
        {
            return _dbSet.FirstOrDefaultAsync(condition);
        }
        // Update the chosen entity 
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
        // Delete the chosen entity
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        // Delete the list of chosen entity
        public void Delete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        // When the entity create a new field, this function need to be called
        public async void UpdateField(string field, dynamic value = null)
        {
            var lstObj = await _dbSet.ToListAsync();
            if (lstObj != null)
            {
                foreach(var obj in lstObj)
                {
                    var propertyInfo = obj.GetType().GetProperty(field);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(obj, value);
                    }
                }
                _dbSet.UpdateRange(lstObj);
            }
            await _context.SaveChangesAsync();
        }
        
    }
}
