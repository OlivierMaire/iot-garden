using iot_garden_shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden.ViewModels
{

    public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class SensorDataViewModel
    {
        //public ObservableCollection<Model> Data { get; set; }

        //public SensorDataViewModel()
        //{
        //    Data = new ObservableCollection<Model>()
        //{
        //    new Model { X = 0, Y = 170 },
        //    new Model { X = 1, Y = 96 },
        //    new Model { X = 2, Y = 65 },
        //    new Model { X = 3, Y = 182 },
        //    new Model { X = 4, Y = 134 }
        //};
        //}

        public ObservableCollection<SensorData> Data { get; set; }

        public SensorDataViewModel()
        {
            Data = new ObservableCollection<SensorData>()
            {
                //new SensorData() { Timestamp = DateTime.Now, Value = 12 },
                //new SensorData() { Timestamp = DateTime.Now, Value = 13 },
                //new SensorData() { Timestamp = DateTime.Now, Value = 14 },
                //new SensorData() { Timestamp = DateTime.Now, Value = 15 },
                //new SensorData() { Timestamp = DateTime.Now, Value = 16 },
            };
        }

    }
}
