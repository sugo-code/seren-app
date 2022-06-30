using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Model
{
    public class DeviceDataId
    {
        public Guid DeviceId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public class DeviceData : AEntityBase<DeviceDataId>
    {
        public double BodyTemperature { get; set; }
        public double BloodPressure { get; set; }
        public double BloodOxygen { get; set; }
        public double Battery { get; set; }
        public int HeartFrequency { get; set; }
        public int WalkCount { get; set; }
        public bool Sleeping { get; set; }
        public bool Fallen { get; set; }
    }
}
