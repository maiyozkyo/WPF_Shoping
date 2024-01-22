using Shoping.Data_Access.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DB.UnitOfWork
{
    interface IUnitOfWork<TEntity> : IDisposable where TEntity : class
    {
        public Repository<TEntity> _repository { get; set; }
        public Task<int> SaveChangeAsync();
    }
}
