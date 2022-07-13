using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SerenApp.Core.Interfaces;
using SerenApp.Infrastructure.DAL.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(SugoCode.QueueTrigger.Startup))]


namespace SugoCode.QueueTrigger
{
    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(config.GetConnectionString("AzureSql"));
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        }

        public class DbInitializer : IWebJobsStartup
        {
            public void Configure(IWebJobsBuilder builder)
            {
                builder.AddExtension<DbConfigProvider>();
            }
        }

        public class DbConfigProvider : IExtensionConfigProvider
        {
            private readonly IServiceScopeFactory _scopeFactory;

            public DbConfigProvider(IServiceScopeFactory scopeFactory)
            {
                _scopeFactory = scopeFactory;
            }

            public void Initialize(ExtensionConfigContext context)
            {
                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.Migrate();
            }
        }
    }
}
