using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DB.MongoDB
{
    public class MongoDBContext<TEntity> : DbContext where TEntity : class
    {
        public DbSet<TEntity> _dbSet { get; set; }
        public string _connectionString { get; set; }

        public MongoDBContext(IConfiguration iConfiguration) {
            _connectionString = iConfiguration.GetSection("MongoDatabase").Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMongoDB(_connectionString, typeof(TEntity).Name);
        }
    }
}
