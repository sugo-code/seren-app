using Microsoft.EntityFrameworkCore;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL
{
    public abstract class AContextBase : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceData> DeviceData { get; set; }
    }
}
