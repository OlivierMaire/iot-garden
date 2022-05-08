using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden.Models
{
    public class GardenSetting
    {
        public string Name { get; set; }
        public List<SensorSetting> Sensors { get; set; }

    }

    public class SensorSetting
    {
        public string Name { get; set; }
        public SensorType Type { get; set; }
        public SensorPort Port { get; set; }

    }

    public enum SensorType
    {
        Temp_Humidity,
        Relay,
        Moisture,
        Light
    }

    public enum SensorPort
    {
        D2,
        D3,
        D4,
        D5,
        D6,
        D7,
        D8,
        A0,
        A1,
        A2,
        I2C1,
        I2C2,
        I2C3,
        RPISER,
        SERIAL

    }

    }
