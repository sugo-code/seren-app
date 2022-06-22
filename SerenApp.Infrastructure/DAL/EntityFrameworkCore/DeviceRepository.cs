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
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AppDbContext context;
        public DeviceRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Device> Delete(Device item)
        {
            context.Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Device>> GetAll()
        {
            return await context.Devices.ToListAsync();
        }

        public async Task<Device> GetById(Guid id)
        {
            return await context.Devices.FirstAsync(d => d.ID == id);
        }

        public async Task<Device> Insert(Device item)
        {
            await context.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<Device> Update(Device item)
        {
            context.Update(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
