using Google.Cloud.Firestore;
using iot_garden.Extensions;
using iot_garden.Models;

namespace iot_garden.Services
{
    public class SettingService
    {
        public GardenSetting Settings { get; set; }

        private readonly FirestoreService _firestore;
        //private FirestoreDb _firestoreDb;
        private Dictionary<string, object> _settingData;
        public SettingService(FirestoreService firestore)
        {
            _firestore = firestore;
            Settings = new GardenSetting();
        }

        public async Task<T> GetValue<T>(string name)
        {
            //if (_firestoreDb == null)
            //    _firestoreDb = await _firestore.GetDb();

            if (_settingData == null)
                await LoadSettings(overrideSettings: false);

            return (T)_settingData[name];
        }
         
        public async Task SaveSettings()
        {
            //var json = System.Text.Json.JsonSerializer.Serialize(Settings);

            var firestoreDb = await _firestore.GetDb();

            DocumentReference docRef = firestoreDb.Collection("settings").Document("setting");
            await docRef.SetAsync(Settings);
        }

        public async Task LoadSettings(bool overrideSettings = false)
        {
            var firestoreDb = await _firestore.GetDb();

            DocumentReference docRef = firestoreDb.Collection("settings").Document("setting");
            DocumentSnapshot docSnapshot = await docRef.GetSnapshotAsync();

            _settingData = docSnapshot.ToDictionary();
            if (overrideSettings)
                Settings = docSnapshot.ToDictionary().ToObject<GardenSetting>();
        }
    }
}