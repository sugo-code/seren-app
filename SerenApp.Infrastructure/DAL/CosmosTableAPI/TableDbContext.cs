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

        //private readonly string conn = Environment.GetEnvironmentVariable("CosmosTableConn");
        //private readonly string tableName = Environment.GetEnvironmentVariable("CosmosTableName");
        //private readonly string accountName = Environment.GetEnvironmentVariable("AccountName");
        //private readonly string endpoint = Environment.GetEnvironmentVariable("Endpoint");
        //private readonly string primaryKey = Environment.GetEnvironmentVariable("PrimaryKey");

        private readonly string conn = "DefaultEndpointsProtocol=https;AccountName=sugocode-cosmosdb-account-table;AccountKey=0hfUq9PAr7xHZ2PyLQtEfxONoL5h0mxLKKMD4YmLV5eMeKn2Fcd7kRUZkg4MLUXYFQl4eQbRhZ5MexLR5ZNZqg==;TableEndpoint=https://sugocode-cosmosdb-account-table.table.cosmos.azure.com:443/;";
        private readonly string tableName = "sugocode-db-table";
        private readonly string accountName = "sugocode-cosmosdb-account-table";
        private readonly string endpoint = "https://sugocode-cosmosdb-account-table.table.cosmos.azure.com:443/";
        private readonly string primaryKey = "0hfUq9PAr7xHZ2PyLQtEfxONoL5h0mxLKKMD4YmLV5eMeKn2Fcd7kRUZkg4MLUXYFQl4eQbRhZ5MexLR5ZNZqg==";

        private readonly TableClient client;

        public TableDbContext() {
            this.client = new TableClient(
            new Uri(endpoint),
            tableName,
            new TableSharedKeyCredential(accountName, primaryKey));
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
