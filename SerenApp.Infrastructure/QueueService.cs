using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SerenApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure
{
    public class QueueService : IQueueService
    {
        private readonly ServiceBusClient client;
        private readonly ServiceBusSender sender;
        private readonly string queueName = "sugocode-servicebus-alert";
        private readonly string conn;
        private readonly IConfiguration _config;
        private readonly ILogger<QueueService> logger;

        public QueueService(IConfiguration config, ILogger<QueueService> logger)
        {
            _config = config;
            conn = _config.GetConnectionString("ServiceBusQueue");
            client = new ServiceBusClient(conn);
            sender = client.CreateSender(queueName);
            this.logger = logger;
        }

        public async Task<bool> SendMessageToQueueAsync(string message)
        {

            ServiceBusMessage mess = new ServiceBusMessage(message);
            
            try
            {
                await sender.SendMessageAsync(mess);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                logger.LogError("Error: " + ex.Message + "StackTrace: " + ex.StackTrace);
                return false;
            }
        }
    }
}
