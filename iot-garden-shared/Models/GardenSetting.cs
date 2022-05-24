using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden_shared.Models
{
    [FirestoreData]
    public class GardenSetting
    {
        public GardenSetting()
        {
            Sensors = new List<SensorSetting>();
        }
        [FirestoreProperty]
        public string Name { get; set; }
        
        [FirestoreProperty]
        public List<SensorSetting> Sensors { get; set; }

    }
    [FirestoreData]

    public class SensorSetting
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public SensorType Type { get; set; }
        [FirestoreProperty]
        public SensorPort Port { get; set; }

        [FirestoreProperty]
        public bool Displayed { get; set; }
    }

    public enum SensorType
    {
        Temperature = 0,
        Humidity = 1,
        Moisture = 2,
        Light =3,
        Relay = 9,

    }

    public enum SensorPort
    {
        D2=0,
        D3=1,
        D4=2,
        D5=3,
        D6=4,
        D7=5,
        D8=6,
        A0=7,
        A1=8,
        A2=9,
        I2C1=10,
        I2C2=11,
        I2C3=12,
        RPISER=13,
        SERIAL

    }

    }
