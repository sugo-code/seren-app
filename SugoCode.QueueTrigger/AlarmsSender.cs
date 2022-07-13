using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Sms;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SerenApp.Core.Interfaces;
using Vonage;
using Vonage.Request;

namespace SugoCode.QueueTrigger
{

    public class AlarmsSender
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration config;
        // COMMUNICATION_SERVICES_CONNECTION_STRING=endpoint=https://sugocode-communication-service.communication.azure.com//;accesskey=B4spR//dQzjRhxOb0qFgBbu5SiXI5COtdvvP//gE3TiALjrJ42n+bbG8aN+rCkeIwsGTtU+xOQoeixbnc7iZYjBw==
        //private readonly string comm_service_conn = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
        //private readonly string comm_service_conn = "endpoint=https://sugocode-communication-service.communication.azure.com/;accesskey=B4spR/dQzjRhxOb0qFgBbu5SiXI5COtdvvP/gE3TiALjrJ42n+bbG8aN+rCkeIwsGTtU+xOQoeixbnc7iZYjBw==";
        private const string mailAndreaZanetti = "andrea.zanetti2892@gmail.com";

        public AlarmsSender(IDeviceRepository deviceRepo,
            IUserRepository userRepo, ILogger<AlarmsSender> logger,
            IConfiguration configuration)
        {
            _deviceRepo = deviceRepo;
            _userRepo = userRepo;
            config = configuration;
        }

        [FunctionName("AlarmsSender")]
        public async Task Run([ServiceBusTrigger("sugocode-servicebus-alert", Connection = "ServiceBusQueueConn")] string myQueueItem, ILogger log)
        {
            var exceptions = new List<Exception>();
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var device = await _deviceRepo.GetById(Guid.Parse(myQueueItem.Split(" ")[0]));
            var user = await _userRepo.GetById(device.User.ID);

            var credentials = Credentials.FromApiKeyAndSecret(
                config["SmsApiKey"],
                config["SmsApiSecret"]
                );
            var vonageClient = new VonageClient(credentials);

            if (myQueueItem.Split(" ")[1] == "Battery")
            {
                try
                {
                    var response = vonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
                    {
                        To = user.PhoneNumber,
                        From = "Seren App Admin Alert",
                        Text = $"The device's battery of {device.Name} is below 20%",
                        Type = Vonage.Messaging.SmsType.unicode

                    });
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
                    exceptions.Add(ex);
                }
            }

            else if (myQueueItem.Split(" ")[1] == "Fallen")
            {
                try
                {
                    var response1 = vonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
                    {
                        To = user.PhoneNumber,
                        From = "Seren App Admin Alert",
                        Text = $"The person with device {device.Name} is fallen",
                        Type = Vonage.Messaging.SmsType.unicode
                    });
                    var response2 = vonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
                    {
                        To = user.SecureContactPhoneNumber,
                        From = "Seren App Admin Alert",
                        Text = $"The person with device {device.Name} is fallen",
                        Type = Vonage.Messaging.SmsType.unicode
                    });

                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
                    exceptions.Add(ex);
                }
            }
            await Task.Delay(5000);
            await Task.Yield();

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
