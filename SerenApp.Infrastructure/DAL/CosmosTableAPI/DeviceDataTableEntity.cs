using Azure;
using Azure.Data.Tables;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL.CosmosTableAPI
{
    public class DeviceDataTableEntity : ITableEntity
    {

        public string PartitionKey { get; set; }
        [Key]
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public double Battery { get; set; }
        public double BodyTemperature { get; set; }
        public double BloodPressure { get; set; }
        public double BloodOxygen { get; set; }
        public int HeartFrequency { get; set; }
        public int WalkCount { get; set; }
        public bool Sleeping { get; set; }
        public bool Fallen { get; set; }

        public static DeviceDataTableEntity FromDeviceData(DeviceData d) {
            
            return new DeviceDataTableEntity
            {
                PartitionKey = d.ID.DeviceId.ToString(),
                RowKey = d.ID.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"),
                Timestamp = DateTimeOffset.Parse(d.ID.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffffff")),
                Battery = d.Battery,
                BloodOxygen = d.BloodOxygen,
                BloodPressure = d.BloodPressure,
                BodyTemperature = d.BodyTemperature,
                Fallen = d.Fallen,
                HeartFrequency = d.HeartFrequency,
                Sleeping = d.Sleeping,
                WalkCount = d.WalkCount
            };
        }

        public DeviceData ToDeviceData() {
            return new DeviceData
            {
                ID = new DeviceDataId
                {
                    DeviceId = Guid.Parse(PartitionKey),
                    Timestamp = DateTime.Parse(RowKey)
                },
                Battery = Battery,
                BloodOxygen = BloodOxygen,
                BloodPressure = BloodPressure,
                BodyTemperature = BodyTemperature,
                Fallen = Fallen,
                HeartFrequency = HeartFrequency,
                Sleeping = Sleeping,
                WalkCount = WalkCount
            };
        }
    }
}
