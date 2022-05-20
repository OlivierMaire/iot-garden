using iot_garden_shared.Models;

namespace iot_garden_server.Services
{

    public class ShareService
    {

        public ShareService()
        {

        }

        public GardenSetting Garden { get; set; }
        public double? LastHumidity { get; set; }
    }

}