using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SerenApp.Core.Utility
{
    public static class MapToDeviceDataTable
    {
        public static DeviceData Map(JsonDocument data)
        {
            var obj = data.RootElement;

            return new DeviceData
            {
                ID = new DeviceDataId
                {
                    DeviceId = Guid.Parse(obj.GetProperty("deviceId").GetString()),
                    Timestamp = DateTimeOffset.Parse(obj.GetProperty("timeStamp").GetString())
                },
                Battery = obj.GetProperty("batteryLvl").GetDouble(),
                BodyTemperature = obj.GetProperty("bodyTemp").GetDouble(),
                BloodPressure = obj.GetProperty("bloodPrs").GetInt32(),
                BloodOxygen = obj.GetProperty("bloodOxg").GetInt32(),
                HeartFrequency = obj.GetProperty("heartFrq").GetInt32(),
                WalkCount = obj.GetProperty("walkCount").GetInt32(),
                Sleeping = Boolean.Parse(obj.GetProperty("isSleeping").GetString()),
                Fallen = Boolean.Parse(obj.GetProperty("isFallen").GetString()),
                Serendipity = obj.GetProperty("serendipityLvl").GetInt32()
            };
        }
    }
}
