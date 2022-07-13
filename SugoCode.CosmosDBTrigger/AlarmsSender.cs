using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Communication.Sms;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SerenApp.Core.Interfaces;

namespace SugoCode.CosmosDBTrigger
{
    public class AlarmsSender
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly IUserRepository _userRepo;
        private readonly string comm_service_conn = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
        private const string adminSugoCode = "3454833880";

        [FunctionName("AlarmsSender")]
        public async Task Run([CosmosDBTrigger(
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
                        var device = await _deviceRepo.GetById(Guid.Parse(item.GetPropertyValue<string>("ParitionKey")));
                        var user = await _userRepo.GetById(device.User.ID);

                        SmsClient smsClient = new SmsClient(comm_service_conn);

                        SmsSendResult sendResult = await smsClient.SendAsync(
                            from: adminSugoCode,
                            to: user.PhoneNumber,
                            message: "Allarmi avviati"
                            );

                        if (sendResult.Successful) log.LogInformation($"{sendResult.ToString()}");
                        else log.LogInformation(sendResult.ToString());
                    }
                }
            }
        }
    }
}
