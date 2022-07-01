using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SugoCode.CosmosDBTrigger
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([CosmosDBTrigger(
            databaseName: "TablesDB",
            collectionName: "sugocode-db-table",
            ConnectionStringSetting = "CosmosTableConn",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                foreach (var item in input)
                {
                    var battery = item.GetPropertyValue<int>("Battery");
                    var isFallen = item.GetPropertyValue<bool>("Fallen");
                    if(battery < 20 || isFallen)
                    {

                    }
                }
            }
        }
    }
}
