using Google.Api.Gax.Grpc.GrpcNetClient;
using Google.Cloud.Firestore;

namespace iot_garden;

public partial class SettingPage : ContentPage
{
	int count = 0;

	public SettingPage()
	{
		InitializeComponent();


	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		CounterLabel.Text = $"Current count: {count}";

		SemanticScreenReader.Announce(CounterLabel.Text);


		using var stream = await FileSystem.OpenAppPackageFileAsync("iot-garden-e5771-1a009c31dc84.json");
		using var reader = new StreamReader(stream);

		var contents = reader.ReadToEnd();
		var builder = new FirestoreDbBuilder()
		{
			ProjectId = "iot-garden-e5771",
			GrpcAdapter = GrpcNetClientAdapter.Default,
			JsonCredentials = contents
		};


		FirestoreDb db = builder.Build();
//		var builder = new Google.Cloud.Firestore.V1.FirestoreClientBuilder { JsonCredentials = contents };

//		FirestoreDb db = FirestoreDb.Create("iot-garden-e5771", builder.Build());
		// Create a document with a random ID in the "users" collection.
		CollectionReference collection = db.Collection("settings");
		//DocumentReference document = await collection.AddAsync(new { Name = new { First = "Ada", Last = "Lovelace" }, Born = 1815 });
		// Query the collection for all documents where doc.Born < 1900.
		Query query = collection.Limit(1);//.WhereLessThan("Born", 1900);
		QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
		var docu = querySnapshot.Documents.FirstOrDefault();

		Dictionary<string, object> data = docu.ToDictionary();
		DataText.Text = "";
		foreach (var item in data)
        {
			DataText.Text += $"{item.Key} : {item.Value} \r\n";

		}

	}
}

