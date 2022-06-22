using Newtonsoft.Json.Linq;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Utility
{
    public static class MapToDeviceDataTable
    {
        public static DeviceData Map(JObject data)
        {
            DeviceData deviceData = new DeviceData();

            deviceData.ID = new DeviceDataId
            {
                DeviceId = Guid.Parse(data["deviceId"].ToString()),
                Timestamp = DateTime.Parse(data["timeStamp"].ToString())
            };

            deviceData.Battery = double.Parse((string)data["battery"]);
            deviceData.BodyTemperature = double.Parse((string)data["bodyTemp"]);
            deviceData.BloodPressure = int.Parse((string)data["bloodPrs"]);
            deviceData.BloodOxygen = int.Parse((string)data["bloodOxg"]);
            deviceData.HeartFrequency = int.Parse((string)data["heartFrq"]);
            deviceData.WalkCount = int.Parse((string)data["walkCount"]);
            deviceData.Sleeping = bool.Parse((string)data["isSleeping"]);
            deviceData.Fallen = bool.Parse((string)data["isFallen"]);

            return deviceData;
        }
    }
}
