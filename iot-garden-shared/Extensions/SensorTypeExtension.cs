using iot_garden_shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden_shared.Extensions
{
    public static class SensorTypeExtension
    {

        public static String AsIconName(this SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Temperature:
                    return "icons_temperature.png";
                case SensorType.Humidity:
                    return "icons_temperature.png";
                case SensorType.Relay:
                    return "icons_pump off.png";
                case SensorType.Moisture:
                    return "icons_moisture.png";
                case SensorType.Light:
                    return "icons_brightness.png";
                default:
                    return "";
            }
        }

    }
}
