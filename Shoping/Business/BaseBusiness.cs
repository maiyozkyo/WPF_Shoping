using Microsoft.EntityFrameworkCore;
using Shoping.Data_Access.DB.MongoDB;
using Shoping.Data_Access.DB.UnitOfWork;
using Shoping.Data_Access.Repo;

namespace Shoping.Business
{
    public class BaseBusiness
    {
        public UnitOfWork UnitOfWork { get; set; }
        public Repository Repository { get; set; }

        public BaseBusiness(string _dbName)
        {
            var iConfiguration = App.Configuration;
            var type = iConfiguration.GetSection("DBType").Value;
            DbContext dbContext = null;
            switch (type)
            {
                case "MongoDB":
                    {
                        dbContext = new MongoDBContext(iConfiguration, _dbName);
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
            UnitOfWork = new UnitOfWork(dbContext);
            Repository = UnitOfWork.Repository;
        }
    }
}
