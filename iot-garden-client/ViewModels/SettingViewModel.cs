using Google.Cloud.Firestore;
using iot_garden.Extensions;
using iot_garden.Models;
using iot_garden.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iot_garden.ViewModels
{
    public class SettingViewModel : INotifyPropertyChanged
    {
        private readonly SettingService _setting;
        public GardenSetting Settings { get; set; }

        //private readonly FirestoreService _firestore;
        //private FirestoreDb _firestoreDb;
        public SettingViewModel(SettingService setting)
        {
            _setting = setting;
            Settings = new GardenSetting();


            LoadData = new Command(async () =>
            {
                await LoadSettings();
            });


            SaveData = new Command(async () =>
            {
                await SaveSettings();
            }); 
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand LoadData { get; private set; }
        public ICommand SaveData { get; private set; } 

        public string Name
        {
            get
            {
                return Settings.Name;
            }
            set
            {
                if (Settings.Name != value)
                {
                    Settings.Name = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
        }

        public List<SensorSetting> Sensors
        {
            get
            {
                return Settings.Sensors;
            }
            set
            {
                if (Settings.Sensors != value)
                {
                    Settings.Sensors = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Sensors"));
                    }
                }
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SaveSettings()
        {

            await _setting.SaveSettings(Settings);

        }

        public async Task LoadSettings()
        {
            Settings = await _setting.LoadSettings(true);

            PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            PropertyChanged(this, new PropertyChangedEventArgs("Sensors"));

        }

         
    }

}
