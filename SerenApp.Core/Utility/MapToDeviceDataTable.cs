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
        public static DeviceDataTable Map(JObject data)
        {
            DeviceDataTable deviceDataTable = new DeviceDataTable();

            deviceDataTable.PartitionKey = data["deviceId"].ToString();
            deviceDataTable.RowKey = Guid.NewGuid().ToString();
            deviceDataTable.Timestamp = DateTimeOffset.Parse(data["timeStamp"].ToString());
            deviceDataTable.BodyTemperature = double.Parse((string)data["bodyTemp"]);
            deviceDataTable.BloodPressure = int.Parse((string)data["bloodPrs"]);
            deviceDataTable.BloodOxygen = int.Parse((string)data["bloodOxg"]);
            deviceDataTable.HeartFrequency = int.Parse((string)data["heartFrq"]);
            deviceDataTable.WalkCount = int.Parse((string)data["walkCount"]);
            deviceDataTable.Sleeping = bool.Parse((string)data["isSleeping"]);
            deviceDataTable.Fallen = bool.Parse((string)data["isFallen"]);

            return deviceDataTable;
        }
    }
}
