using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shoping.Data_Access.DB.MongoDB;
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
        public Repository<TEntity> Repository { get; set; }

        public BaseBusiness(IConfiguration iConfiguration)
        {
            var type = iConfiguration.GetSection("DBType").Value;
            DbContext dbContext = null;
            switch (type)
            {
                case "MongoDB":
                    {
                        dbContext = new MongoDBContext<TEntity>(iConfiguration);
                        break;
                    }
                case "Posgrest":
                    {
                        break;
                    }
                case "MySQL":
                    {

                        break;
                    }
                    default :
                    {
                        throw new Exception("Không có loại DB");
                    }
            }
            UnitOfWork = new UnitOfWork<TEntity>(dbContext);
            Repository = UnitOfWork.GetRepository();
        }
    }
}
