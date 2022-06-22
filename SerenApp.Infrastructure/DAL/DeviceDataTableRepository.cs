using Microsoft.EntityFrameworkCore;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL
{
    public class DeviceDataTableRepository : IDeviceDataTableRepository
    {
        private readonly AppDbContext context;
        public DeviceDataTableRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<DeviceDataTable> Delete(DeviceDataTable item)
        {
            context.DeviceDataTable.Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<DeviceDataTable>> GetAll()
        {
            return await context.DeviceDataTable.ToListAsync();
        }

        public async Task<DeviceDataTable> GetById(string id)
        {
            return await context.DeviceDataTable.FirstAsync(d => d.PartitionKey == id);
        }

        public async Task<DeviceDataTable> Insert(DeviceDataTable item)
        {
            await context.DeviceDataTable.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<int> InsertManyAsync(IEnumerable<DeviceDataTable> devices)
        {
            await context.DeviceDataTable.AddRangeAsync(devices);
            await context.SaveChangesAsync();
            return devices.Count();
        }

        public async Task<DeviceDataTable> Update(DeviceDataTable item)
        {
            context.DeviceDataTable.Update(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
