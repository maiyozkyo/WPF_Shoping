using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DB.MongoDB
{
    public class MongoDBContext : DbContext
    {
        public string _connectionString { get; set; }
        public string _databaseName { get; set; }

        public MongoDBContext(IConfiguration iConfiguration, string databaseName)
        {
            _connectionString = iConfiguration.GetSection("MongoDatabase").Value;
            _databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMongoDB(_connectionString, _databaseName);
        }
    }
}
