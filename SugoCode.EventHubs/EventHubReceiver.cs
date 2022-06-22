using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using SerenApp.Core.Utility;
using SerenApp.Infrastructure.Services.CosmosTableAPI;

namespace SugoCode.EventHubs
{
    public class EventHubReceiver
    {
        private readonly ITableAPI _tableClient;

        public EventHubReceiver(ITableAPI tableAPI)
        {
            _tableClient = tableAPI;
        }

        [FunctionName("EventHubReceiver")]
        public async Task Run([EventHubTrigger("device/*/message/events", Connection = "EventHubSubConn")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            var deviceDataTelemetries = new List<DeviceDataTable>();

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
                            var dataBodyObject = JObject.Parse(messageBody);
                            DeviceDataTable deviceData = MapToDeviceDataTable.Map(dataBodyObject);
                            log.LogInformation($"Mapped To: \n{deviceData}");
                            deviceDataTelemetries.Add(deviceData);
                        }
                        catch (Exception e)
                        {
                            exceptions.Add(e);
                        }
                    }
                    else if(messageBody.StartsWith("["))
                    {
                        log.LogInformation("Parsing Data to JArray... ");
                        JArray parsedArr = JArray.Parse(messageBody);

                        foreach (JObject parsedObject in parsedArr.Children<JObject>())
                        {
                            if (parsedObject["data"]["body"] is not null)
                            {
                                log.LogInformation("Deserialize to DeviceData");
                                log.LogInformation(parsedObject["data"]["body"].ToString());

                                var dataBodyObject = JObject.Parse(parsedObject["data"]["body"].ToString());
                                DeviceDataTable deviceData = MapToDeviceDataTable.Map(dataBodyObject);
                                deviceDataTelemetries.Add(deviceData);
                            }
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
            if (deviceDataTelemetries.Count() == 10)
            {
                try
                {
                    await _tableClient.InsertManyAsync(deviceDataTelemetries);
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
    }
}
