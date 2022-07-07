using Microsoft.EntityFrameworkCore;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL.EntityFrameworkCore
{
    public class DeviceReportRepository : IDeviceReportRepository
    {
        private readonly AppDbContext context;
        public DeviceReportRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<DeviceReport> Delete(DeviceReport item)
        {
            context.Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<DeviceReport>> GetAll()
        {
            return await context.DeviceReports.ToListAsync();
        }

        public async Task<DeviceReport> GetById(DateTimeOffset id)
        {
            return await context.DeviceReports.FirstAsync(d => d.ID == id);
        }

        public async Task<DeviceReport> Insert(DeviceReport item)
        {
            await context.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<DeviceReport> Update(DeviceReport item)
        {
            context.Update(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
