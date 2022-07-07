using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Model
{
    public class DeviceReport : AEntityBase<DateTimeOffset>
    {
        public double BodyTemperatureAvg { get; set; }
        public double BloodPressureAvg { get; set; }
        public double BloodOxygenAvg { get; set; }
        public double BatteryAvg { get; set; }
        public int HeartFrequencyAvg { get; set; }
    }
}
