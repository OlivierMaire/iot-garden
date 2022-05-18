using iot_garden.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden.ViewModels
{
    public class SensorDataViewModel
    {
        public ObservableCollection<SensorData> Data { get; set; }

        public SensorDataViewModel()
        {
            Data = new ObservableCollection<SensorData>()
            {
                new SensorData() { Timestamp = DateTime.Now, Value = 12 },
                new SensorData() { Timestamp = DateTime.Now, Value = 13 },
                new SensorData() { Timestamp = DateTime.Now, Value = 14 },
                new SensorData() { Timestamp = DateTime.Now, Value = 15 },
                new SensorData() { Timestamp = DateTime.Now, Value = 16 },
            };
        }

    }
}
