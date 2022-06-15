using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL.InMemory
{
    public class InMemoryContext : AContextBase
    {
        private readonly string databaseName;

        public InMemoryContext(IConfiguration config)
        {
            this.databaseName = config[Config.DatabaseNameKey];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName);
        }
    }
}
