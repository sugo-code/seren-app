using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Model
{
    public class DeviceData : AEntityBase<DateTime>
    {
        public double BodyTemperature { get; set; }
        public double BloodPressure { get; set; }
        public double BloodOxygen { get; set; }
        public double HeartFrequency { get; set; }
        public bool Sleeping { get; set; }
        public bool Fallen { get; set; }
        public Device Device { get; set; }
    }
}
