using Google.Api.Gax.Grpc.GrpcNetClient;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iot_garden_shared.Services;

namespace iot_garden_server.Services
{
    public class FirestoreService : IFirestoreService
    {

        public async Task<FirestoreDb> GetDb()
        {

			using var stream = File.OpenRead("./config/iot-garden-e5771-1a009c31dc84.json");
			using var reader = new StreamReader(stream);

			var contents = reader.ReadToEnd();
			var builder = new FirestoreDbBuilder()
			{
				ProjectId = "iot-garden-e5771",
				GrpcAdapter = GrpcNetClientAdapter.Default,
				JsonCredentials = contents
			};

			FirestoreDb db = builder.Build();

			return db;
		}
    }
}
