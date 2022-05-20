using Google.Cloud.Firestore;
// using iot_garden.Extensions;
using iot_garden_shared.Models;

namespace iot_garden_shared.Services
{
    public class SettingService
    {
        public GardenSetting Settings { get; set; }

        private readonly IFirestoreService _firestore;
        //private FirestoreDb _firestoreDb;
        //private Dictionary<string, object> _settingData;
        public SettingService(IFirestoreService firestore)
        {
            _firestore = firestore;
            //Settings = new GardenSetting();
        }

        //public async Task<T> GetValue<T>(string name)
        //{
        //    //if (_firestoreDb == null)
        //    //    _firestoreDb = await _firestore.GetDb();

        //    if (_settingData == null)
        //        await LoadSettings(overrideSettings: false);

        //    return (T)_settingData[name];
        //}

        public async Task SaveSettings(GardenSetting newSettings)
        {
            //var json = System.Text.Json.JsonSerializer.Serialize(Settings);

            var firestoreDb = await _firestore.GetDb();

            DocumentReference docRef = firestoreDb.Collection("settings").Document("setting");
            await docRef.SetAsync(newSettings);
            Settings = newSettings;
        }

        public async Task<GardenSetting> LoadSettings(bool force = false)
        {

            //_settingData = docSnapshot.ToDictionary();
            if (force || Settings == null)
            {
                var firestoreDb = await _firestore.GetDb();

                DocumentReference docRef = firestoreDb.Collection("settings").Document("setting");
                DocumentSnapshot docSnapshot = await docRef.GetSnapshotAsync();

                //Settings = docSnapshot.ToDictionary().ToObject<GardenSetting>();
                Settings = docSnapshot.ConvertTo<GardenSetting>();
                               

            }
            return Settings ?? new GardenSetting();
        }

        public async Task StartListeningForSensor(string sensorId, Func<SensorData, Task> callback)
        {
            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var db = await _firestore.GetDb();

            //Query query = db.Collection("realtime").WhereEqualTo("SensorId", sensorId).OrderBy("Timestamp").LimitToLast(3);
            //var snap = await query.GetSnapshotAsync();
            //foreach( var item in snap)
            //{
            //    try
            //    {
            //        SensorData data = item.ConvertTo<SensorData>();
            //        await callback.Invoke(data);

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"new document found error {ex.Message}");

            //    }
            //}

            Query query = db.Collection("realtime").WhereEqualTo("SensorId", sensorId).OrderBy("Timestamp").LimitToLast(10);
            Console.WriteLine($"Listening for sensor {sensorId}");
            FirestoreChangeListener listener = query.Listen( async snapshot =>
            {

                foreach (DocumentChange change in snapshot.Changes)
                {
                    if (change.ChangeType.ToString() == "Added")
                    {
                        //Console.WriteLine($"new document found {documentSnapshot.Id}");


                        try
                        {
                            SensorData data = change.Document.ConvertTo<SensorData>();
                            await callback.Invoke(data);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"new document found error {ex.Message}");

                        }
                    }
                    else if (change.ChangeType.ToString() == "Modified")
                    {
                    }
                    else if (change.ChangeType.ToString() == "Removed")
                    {
                    }

                }

                //foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                //{


                //    //Console.WriteLine($"new document found {documentSnapshot.Id}");

                //    try
                //    {
                //        SensorData data = documentSnapshot.ConvertTo<SensorData>();
                //        await callback.Invoke(data);

                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine($"new document found error {ex.Message}");

                //    }
                //}
            });
        }

    }
}