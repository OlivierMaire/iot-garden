using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden.Models
{
    [FirestoreData]
    public class SensorData
    {
        [FirestoreProperty]
        public string SensorId { get; set; }

        [FirestoreProperty]
        public double Value { get; set; }

        [FirestoreProperty]
        public DateTime Timestamp { get; set; }
    }
}
