using Microsoft.EntityFrameworkCore;
using Shoping.Data_Access.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DB.UnitOfWork
{
    public class UnitOfWork<TEntity>: IUnitOfWork<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        public Repository<TEntity> _repository { get; set; }

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            _repository = new Repository<TEntity>(dbContext);
        }

        public async Task<int> SaveChangsAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        
        public Repository<TEntity> GetRepository()
        {
            return _repository;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public Task<int> SaveChangeAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
