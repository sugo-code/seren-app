using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SerenApp.Core.Interfaces;
using SerenApp.Infrastructure.DAL;
using SerenApp.Infrastructure.DAL.CosmosTableAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(SugoCode.EventHubs.Startup))]

namespace SugoCode.EventHubs
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //{
            //    var tableName = Environment.GetEnvironmentVariable("CosmosTableName");
            //    options.UseCosmos(Environment.GetEnvironmentVariable("CosmosTableConn"), tableName);
            //});
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddScoped<TableDbContext>(x => {
                var connectionString = config.GetConnectionString("Cosmos");
                var tableName = config["TableName"];
                return new TableDbContext(connectionString, tableName);
            });
            builder.Services.AddScoped<IDeviceDataRepository, DeviceDataRepository>();
            
        }
    }

}
