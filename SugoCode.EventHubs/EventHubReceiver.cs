using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using SerenApp.Core.Utility;
using SerenApp.Infrastructure.DAL.CosmosTableAPI;

namespace SugoCode.EventHubs
{
    public class EventHubReceiver
    {
        private readonly IDeviceDataRepository _repository;
        private readonly IQueueService _queueService;

        public EventHubReceiver(IDeviceDataRepository repository, IQueueService queueService)
        {
            _repository = repository;
            _queueService = queueService;
        }

        [FunctionName("EventHubReceiver")]
        public async Task Run([EventHubTrigger("device/*/message/events", Connection = "EventHubSubConn")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            var deviceDataTelemetries = new List<DeviceData>();

            // Fetching all Events from EventsHub
            log.LogInformation("Reading EventData... ");
            foreach(EventData eventData in events)
            {
                try
                {
                    // Now it's send only the body properties
                    //{
                    //    "key";"value"
                    //}
                    string decodedMessageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    string messageBody = decodedMessageBody.Replace('\'', '"');

                    if (messageBody.Contains("False")) messageBody.Replace("False", "false");
                    if (messageBody.Contains("True")) messageBody.Replace("True", "true");

                    log.LogInformation(messageBody);

                    if (messageBody.StartsWith("{"))
                    {
                        try
                        {
                            //DeviceDataTable deviceDataTab = JsonConvert.DeserializeObject<DeviceDataTable>(messageBody);
                            //log.LogInformation($"Deserialize:\nType: {deviceDataTab}\n");

                            log.LogInformation("Parsing Data to JObject... ");
                            using var dataBodyObject = JsonDocument.Parse(messageBody);
                            DeviceData deviceData = MapToDeviceDataTable.Map(dataBodyObject);
                            log.LogInformation($"Mapped To: \n{deviceData}");
                            deviceDataTelemetries.Add(deviceData);
                            await CheckDataThenSendToQueue(log, deviceData);
                        }
                        catch (Exception e)
                        {
                            exceptions.Add(e);
                        }
                    }
                    else if(messageBody.StartsWith("["))
                    {
                        log.LogInformation("Parsing Data to JArray... ");
                        using var parsedArr = JsonDocument.Parse(messageBody);

                        foreach (JsonElement parsedObject in parsedArr.RootElement.EnumerateArray())
                        {
                            try
                            {
                                using var data = JsonDocument.Parse(parsedObject.GetProperty("data").GetProperty("body").ToString());
                                log.LogInformation("Deserialize to DeviceData");
                                log.LogInformation(data.ToString());
                                var deviceData = MapToDeviceDataTable.Map(data);
                                deviceDataTelemetries.Add(deviceData);
                                await CheckDataThenSendToQueue(log, deviceData);
                            }
                            catch { }
                        }
                    }

                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Uploading data to Cosmos Table
            if (deviceDataTelemetries.Any())
            {
                try
                {
                    await _repository.InsertManyAsync(deviceDataTelemetries);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }


        private async Task CheckDataThenSendToQueue(ILogger log, DeviceData device)
        {
            if(device.Battery < 20)
            {
                var result = await _queueService.SendMessageToQueueAsync($"{device.ID.DeviceId} Battery");

                if (result) log.LogInformation("Battery Message succefully send it to the queue");
                else log.LogError("Something wrong in sending message to the queue");
            }
            if (device.Fallen)
            {
                var result = await _queueService.SendMessageToQueueAsync($"{device.ID.DeviceId} Fallen");

                if (result) log.LogInformation("Fallen Message succefully send it to the queue");
                else log.LogError("Something wrong in sending message to the queue");
            }

            await Task <bool>.Yield();
        }
    }
}
