using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.Services.CosmosTableAPI
{
    public class TableAPI : ITableAPI
    {
        // https://sugocode-cosmosdb-account-table.table.cosmos.azure.com:443/
        // sugocode-cosmosdb-account-table
        // 0hfUq9PAr7xHZ2PyLQtEfxONoL5h0mxLKKMD4YmLV5eMeKn2Fcd7kRUZkg4MLUXYFQl4eQbRhZ5MexLR5ZNZqg==

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

        private async Task<TableClient> CreateOrGetTableAsync()
        {
            //var client = new TableServiceClient(
            //    new Uri(endpoint),
            //    new TableSharedKeyCredential(accountName, primaryKey)
            //);
            
            //await client.CreateTableIfNotExistsAsync(this.tableName);
            //var table = new TableClient(this.conn, this.tableName);

                var tableClient = new TableClient(
                new Uri(endpoint),
                tableName,
                new TableSharedKeyCredential(accountName, primaryKey));
            return tableClient;
        }
        
        public async Task InsertManyAsync(IEnumerable<DeviceDataTable> devicesData)
        {
            try
            {
                var table = await CreateOrGetTableAsync();
                foreach (var deviceData in devicesData)
                {
                    var result = await table.AddEntityAsync(deviceData);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task InsertAsync(DeviceDataTable deviceData)
        {
            try
            {
                var table = await CreateOrGetTableAsync();
                var result = await table.AddEntityAsync(deviceData);
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
