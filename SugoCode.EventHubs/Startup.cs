using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SerenApp.Core.Interfaces;
using SerenApp.Infrastructure.DAL;
using SerenApp.Infrastructure.DAL.CosmosTableAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
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

            builder.Services.AddScoped<TableDbContext>(x => {
                var connectionString = ConfigurationManager.ConnectionStrings["Cosmos"].ToString();
                var tableName = ConfigurationManager.AppSettings["TableName"];
                return new TableDbContext(connectionString, tableName);
            });
            builder.Services.AddScoped<IDeviceDataRepository, DeviceDataRepository>();
            
        }
    }

}
