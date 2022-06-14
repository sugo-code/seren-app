using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Interfaces
{
    public interface IDeviceDataRepository : IRepository<DeviceData, DateTime>
    {
        public Task<IEnumerable<DeviceData>> GetManyByDevice(Device d);
    }
}
