using iot_garden_shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iot_garden_shared.Services;

namespace iot_garden.ViewModels
{
    public class GardenViewModel : INotifyPropertyChanged
    {
        private readonly SettingService _setting;

        public event PropertyChangedEventHandler PropertyChanged;

        //readonly IPublisher<string, SensorData> _publisher;
        public GardenSetting Settings { get; set; }

        public Dictionary<string, List<SensorData>> sensorData { get; set; }

        //private readonly FirestoreService _firestore;
        //private FirestoreDb _firestoreDb;
        public GardenViewModel(SettingService setting/*, IPublisher<string, SensorData> publisher*/)
        {
            _setting = setting;
            //_publisher = publisher;
            Settings = new GardenSetting();


            OnLoaded = new Command(async () =>
            {
                await LoadSettings();
                await StartListening();

            });


        }


         

        public ICommand OnLoaded { get; private set; }


        public async Task LoadSettings()
        {
            Settings = await _setting.LoadSettings();

            PropertyChanged(this, new PropertyChangedEventArgs("GardenName"));
            PropertyChanged(this, new PropertyChangedEventArgs("Sensors"));

        }

        public async Task StartListening()
        {
            if (Settings == null || Settings.Sensors == null)
                return;

            foreach (var sensor in Settings.Sensors)
            {
                await _setting.StartListeningForSensor(sensor.Id, SaveSensorData);
            }
        }

        public async Task SaveSensorData(SensorData data)
        {
            Thread.Sleep(1000);
            if (sensorData == null)
                sensorData = new Dictionary<string, List<SensorData>>();
            if (!sensorData.ContainsKey(data.SensorId))
                sensorData.Add(data.SensorId, new List<SensorData>());
            sensorData[data.SensorId].Add(data);
                Console.WriteLine($"Sensor data received {data.SensorId} Value: {data.Value}.");
            //_publisher.Publish(data.SensorId,data);
            MessagingCenter.Send<GardenViewModel, SensorData>(this, data.SensorId, data);
        }

        public string GardenName
        {
            get => Settings.Name;
        }


        public List<SensorSetting> Sensors
        {
            get => Settings.Sensors;
        }


    }
}
