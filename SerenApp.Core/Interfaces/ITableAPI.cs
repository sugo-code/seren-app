using Azure.Data.Tables;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Interfaces
{
    public interface ITableAPI
    {
        Task InsertAsync(DeviceDataTable deviceData);
        Task InsertManyAsync(IEnumerable<DeviceDataTable> devicesData);
        Task GetByRowKeyAsync(string RKey);
        Task GetByPartitionKeyAsync(string PKey);
    }
}
