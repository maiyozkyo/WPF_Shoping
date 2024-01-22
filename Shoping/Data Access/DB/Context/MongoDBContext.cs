using Microsoft.EntityFrameworkCore;
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

        public MongoDBContext(string connectionString) {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMongoDB(_connectionString, typeof(TEntity).Name);
        }
    }
}
