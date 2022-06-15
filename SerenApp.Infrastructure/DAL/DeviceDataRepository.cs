using Microsoft.EntityFrameworkCore;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using SerenApp.Infrastructure.DAL.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL
{
    public class DeviceDataRepository : IDeviceDataRepository
    {
        private readonly AContextBase context;
        public DeviceDataRepository(AContextBase context)
        {
            this.context = context;
        }

        public async Task<DeviceData> Delete(DeviceData item)
        {
            context.Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<DeviceData>> GetAll()
        {
            return await context.DeviceData.ToListAsync();
        }

        public async Task<DeviceData> GetById(DateTime id)
        {
            return await context.DeviceData.FirstAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DeviceData>> GetManyByDevice(Device d)
        {
            return await context.DeviceData.Where(x => x.Device == d).ToListAsync();
        }

        public async Task<DeviceData> Insert(DeviceData item)
        {
            await context.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<DeviceData> Update(DeviceData item)
        {
            context.Update(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
