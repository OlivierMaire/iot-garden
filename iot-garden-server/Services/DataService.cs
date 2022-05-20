using iot_garden_shared.Models;
using iot_garden_shared.Services;

namespace iot_garden_server.Services;

public class DataService
{

    private readonly IFirestoreService _db;

    public DataService(IFirestoreService db)
    {
        _db = db;
    }

    public async Task UploadData(SensorData data)
    {
        var firestoreDb = await _db.GetDb();
        await firestoreDb.Collection("realtime").AddAsync(data);
    }

}