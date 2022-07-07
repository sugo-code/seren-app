using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Sms;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SerenApp.Core.Interfaces;

namespace SugoCode.QueueTrigger
{

    public class AlarmsSender
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly IUserRepository _userRepo;
        // COMMUNICATION_SERVICES_CONNECTION_STRING=endpoint=https://sugocode-communication-service.communication.azure.com//;accesskey=B4spR//dQzjRhxOb0qFgBbu5SiXI5COtdvvP//gE3TiALjrJ42n+bbG8aN+rCkeIwsGTtU+xOQoeixbnc7iZYjBw==
        //private readonly string comm_service_conn = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
        private readonly string comm_service_conn = "endpoint=https://sugocode-communication-service.communication.azure.com/;accesskey=B4spR/dQzjRhxOb0qFgBbu5SiXI5COtdvvP/gE3TiALjrJ42n+bbG8aN+rCkeIwsGTtU+xOQoeixbnc7iZYjBw==";
        private const string adminSugoCode = "+393454833880";

        public AlarmsSender(IDeviceRepository deviceRepo, IUserRepository userRepo, ILogger<AlarmsSender> logger)
        {
            _deviceRepo = deviceRepo;
            _userRepo = userRepo;
        }

        [FunctionName("AlarmsSender")]
        public async Task Run([ServiceBusTrigger("sugocode-servicebus-queue", Connection = "ServiceBusQueueConn")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var device = await _deviceRepo.GetById(Guid.Parse(myQueueItem.Split(" ")[0]));
            var user = await _userRepo.GetById(device.User.ID);
            SmsClient smsClient = new SmsClient(comm_service_conn);


            if (myQueueItem.Split(" ")[1] == "Battery")
            {
                try
                {
                    SmsSendResult sendResult = await smsClient.SendAsync(
                        from: adminSugoCode,
                        to: user.PhoneNumber,
                        message: "La batteria del tuo briaccialetto è sotto il 20%! Ricaricalo pezzente!"
                    );

                    if (sendResult.Successful) log.LogInformation($"Battery Message was sent: {sendResult}");
                    else log.LogError("Error in sending Battery message" + sendResult.ToString());

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }

            else if (myQueueItem.Split(" ")[1] == "Fallen")
            {
                try
                {
                    Response<IReadOnlyList<SmsSendResult>> sendResult = await smsClient.SendAsync(
                       from: adminSugoCode,
                       to: new string[] { user.PhoneNumber, user.SecureContactPhoneNumber },
                       message: $"La persona del briaccialetto {device.ID} è caduta! Chiama i soccorsi e prega!"
                   );

                    IEnumerable<SmsSendResult> results = sendResult.Value;
                    foreach (SmsSendResult result in results)
                    {
                        if (result.Successful) log.LogInformation($"Fallen Message was sent: {result}");
                        else log.LogError("Error in sending Fallen Message: " + result.ToString());
                    }

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }

            await Task.Yield();
        }
    }
}
