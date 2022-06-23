using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;

namespace SerenApp.Infrastructure.DAL.CosmosTableAPI
{
    public class TableDbContext
    {
        private readonly TableClient client;

        public TableDbContext(string connectionString, string tableName) {

            this.client = new TableClient(connectionString, tableName);
        }

        public async Task InsertManyAsync(IEnumerable<DeviceDataTableEntity> devicesData)
        {
            try
            {
                foreach (var deviceData in devicesData)
                {
                    var result = await client.AddEntityAsync(deviceData);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task InsertAsync(DeviceDataTableEntity deviceData)
        {
            try
            {
                var result = await client.AddEntityAsync(deviceData);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task Remove(DeviceDataTableEntity deviceData)
        {
            try
            {
                var result = await client.DeleteEntityAsync(deviceData.PartitionKey, deviceData.RowKey);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateAsync(DeviceDataTableEntity deviceData)
        {
            try
            {
                var result = await client.UpdateEntityAsync(deviceData, Azure.ETag.All);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<DeviceDataTableEntity>> QueryAsync(Expression<Func<DeviceDataTableEntity, bool>> filter,/* int? maxPerPage = null, IEnumerable<string> select = null,*/ CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = new List<DeviceDataTableEntity>();
            var result = client.QueryAsync(filter);

            try
            {
                await foreach (var item in result.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    list.Add(item);
                }

                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task GetByPartitionKeyAsync(string PKey)
        {
            throw new NotImplementedException();
        }

        public Task GetByRowKeyAsync(string RKey)
        {
            throw new NotImplementedException();
        }
    }
}
