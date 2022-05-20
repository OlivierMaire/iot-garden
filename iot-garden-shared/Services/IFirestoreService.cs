using Google.Api.Gax.Grpc.GrpcNetClient;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden_shared.Services
{
    public interface IFirestoreService
    {
        public Task<FirestoreDb> GetDb();
    }
}
