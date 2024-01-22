using Microsoft.EntityFrameworkCore;
using Shoping.Data_Access.DB.UnitOfWork;
using Shoping.Data_Access.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public class BaseBusiness<TEntity> where TEntity : class
    {
        public UnitOfWork<TEntity> UnitOfWork { get; set; }
        public IRepository<TEntity> Repository { get; set; }

        public BaseBusiness(DbContext dbContext)
        {
            UnitOfWork = new UnitOfWork<TEntity>(dbContext);
            Repository = UnitOfWork
        }
    }
}
