using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SerenApp.Core.Interfaces;

namespace SugoCode.EventHubs
{
    public class EventHubReceiver
    {
        private IDeviceDataRepository _deviceDataRepo;

        public EventHubReceiver(IDeviceDataRepository deviceDataRepo)
        {
            _deviceDataRepo = deviceDataRepo;
        }

        [FunctionName("EventHubReceiver")]
        public async Task Run([EventHubTrigger("device/*/message/events", Connection = "EventHubSubConn")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    log.LogInformation("Reading EventData... ");
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    log.LogInformation("Parsing Data to JArray... ");
                    JArray parsedArr = JArray.Parse(messageBody);
                    JObject bodyTelemetry = new JObject();
                    foreach (JObject parsedObject in parsedArr.Children<JObject>())
                    {
                        if (parsedObject["data"]["body"] is not null)
                        {
                            log.LogInformation("Parsing to JObject data.boby");
                            bodyTelemetry = (JObject)parsedObject["data"]["body"];
                        }
                    }

                    if (!string.IsNullOrEmpty(bodyTelemetry.ToString()))
                    {
                        var conn = Environment.GetEnvironmentVariable("CosmosTable");

                        log.LogInformation("Connecting to Cosmos Table - DeviceTelemetry... ");
                        //TableClient tableClient = new TableClient(conn, "DeviceTelemetry");
                        
                    }
                    else
                    {
                        log.LogInformation("Unable to Find DeviceTelemetry: data/body property is missing!");
                        log.LogInformation("Control the messagge sent by Simulator");
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

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
