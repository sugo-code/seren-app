using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SerenApp.Core.Interfaces;
using SerenApp.Infrastructure.DAL;
using SerenApp.Infrastructure.Services.CosmosTableAPI;
using System;
using System.Collections.Generic;
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

            builder.Services.AddScoped<ITableAPI, TableAPI>();
            
        }
    }

}
