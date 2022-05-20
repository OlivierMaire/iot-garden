using iot_garden.Extensions;
using iot_garden_shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden.ViewModels
{
    public class SensorSettingViewModel : INotifyPropertyChanged
    {
        public SensorSetting SensorData { get; set; }

        public SensorSettingViewModel()
        {
            SensorData = new SensorSetting();
        }

        public string Name
        {
            get
            {
                return SensorData.Name;
            }
            set
            {
                if (SensorData.Name != value)
                {
                    SensorData.Name = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
        }

        public List<string> TypeNames
        {
            get
            {
                return Enum.GetNames(typeof(SensorType)).Select(b => b.SplitCamelCase()).ToList();
            }
        }
         
        public SensorType Type
        {
            get => SensorData.Type;
            set {

                if (SensorData.Type != value)
                {
                    SensorData.Type = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Type"));
                    }
                }
            }
        }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        public List<string> PortNames
        {
            get
            {
                return Enum.GetNames(typeof(SensorPort)).Select(b => b.SplitCamelCase()).ToList();
            }
        }

        public SensorPort Port
        {
            get
            {
                return SensorData.Port;
            }
            set
            {
                if (SensorData.Port != value)
                {
                    SensorData.Port = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Port"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
