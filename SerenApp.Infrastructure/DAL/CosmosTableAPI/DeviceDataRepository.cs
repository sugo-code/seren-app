using Microsoft.EntityFrameworkCore;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL.CosmosTableAPI
{
    public class DeviceDataRepository : IDeviceDataRepository
    {
        private readonly TableDbContext context;
        public DeviceDataRepository(TableDbContext context)
        {
            this.context = context;
        }

        public async Task<DeviceData> Delete(DeviceData item)
        {
            await context.Remove(DeviceDataTableEntity.FromDeviceData(item));
            return item;
        }

        public async Task<IEnumerable<DeviceData>> GetAll()
        {
            var list = await context.QueryAsync(x => true);
            return list.Select(x => x.ToDeviceData());
        }

        public async Task<DeviceData> GetById(DeviceDataId id)
        {
            var data = DeviceDataTableEntity.FromDeviceData( new DeviceData { ID = id });
            var list = await context.QueryAsync(x => x.RowKey == data.RowKey && x.PartitionKey == data.PartitionKey);
            return list.Select(x => x.ToDeviceData()).First();
        }

        public async Task<IEnumerable<DeviceData>> GetManyByDevice(Device d)
        {
            var data = DeviceDataTableEntity.FromDeviceData(new DeviceData { ID = new DeviceDataId { DeviceId = d.ID } });
            var list = await context.QueryAsync(x => x.PartitionKey == data.PartitionKey);
            return list.Select(x => x.ToDeviceData());
        }

        public async Task<DeviceData> Insert(DeviceData item)
        {
            await context.InsertAsync(DeviceDataTableEntity.FromDeviceData(item));
            return item;
        }

        public async Task<int> InsertManyAsync(IEnumerable<DeviceData> devices)
        {
            await context.InsertManyAsync(devices.Select(x => DeviceDataTableEntity.FromDeviceData(x)));
            return devices.Count();
        }

        public async Task<DeviceData> Update(DeviceData item)
        {
            await context.UpdateAsync(DeviceDataTableEntity.FromDeviceData(item));
            return item;
        }
    }
}
