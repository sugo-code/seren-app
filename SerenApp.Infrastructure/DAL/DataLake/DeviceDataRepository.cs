using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL.DataLake
{
    class DeviceDataRepository : IDeviceDataRepository
    {
        public Task<DeviceData> Delete(DeviceData item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeviceData>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DeviceData> GetById(DateTime id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeviceData>> GetManyByDevice(Device d)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceData> Insert(DeviceData item)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceData> Update(DeviceData item)
        {
            throw new NotImplementedException();
        }
    }
}
