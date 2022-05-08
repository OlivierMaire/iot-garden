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
        public GardenSetting Settings { get; set; }

        private readonly FirestoreService _firestore;
        //private FirestoreDb _firestoreDb;
        private Dictionary<string, object> _settingData;
        public SettingViewModel(FirestoreService firestore)
        {
            _firestore = firestore;
            Settings = new GardenSetting();


            LoadData = new Command(async () =>
            {
                await LoadSettings(true);
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


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SaveSettings()
        {
            //var json = System.Text.Json.JsonSerializer.Serialize(Settings);

            var firestoreDb = await _firestore.GetDb();

            DocumentReference docRef = firestoreDb.Collection("settings").Document("setting");
            await docRef.SetAsync(Settings.AsDictionary());
        }

        public async Task LoadSettings(bool overrideSettings = false)
        {
            var firestoreDb = await _firestore.GetDb();

            DocumentReference docRef = firestoreDb.Collection("settings").Document("setting");
            DocumentSnapshot docSnapshot = await docRef.GetSnapshotAsync();

            _settingData = docSnapshot.ToDictionary();
            if (overrideSettings)
            {
                Settings = docSnapshot.ToDictionary().ToObject<GardenSetting>();
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }

        }
    }
}