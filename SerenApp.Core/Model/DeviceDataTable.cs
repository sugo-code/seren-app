using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Model
{
    public class DeviceDataTable : ITableEntity
    {
        
        public string PartitionKey { get; set; }
        [Key]
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public double BodyTemperature { get; set; }
        public double BloodPressure { get; set; }
        public double BloodOxygen { get; set; }
        public int HeartFrequency { get; set; }
        public int WalkCount { get; set; }  
        public bool Sleeping { get; set; }
        public bool Fallen { get; set; }

    }
}
