using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL.Cosmos
{
    public class CosmosContext : AContextBase
    {
        private readonly string connectionString;
        private readonly string databaseName;

        public CosmosContext(IConfiguration config)
        {
            this.connectionString = config.GetConnectionString(Config.CosmosConnectionStringKey);
            this.databaseName = config[Config.DatabaseNameKey];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseCosmos(connectionString, databaseName);
        }
    }
}
