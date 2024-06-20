using Geocoding;
using Geocoding.Google;
using Microsoft.Extensions.Configuration;
using Address = FarmersMarketplace.Domain.Address;

namespace FarmersMarketplace.Application.Helpers
{
    public class CoordinateHelper
    {
        protected IConfiguration Configuration { get; set; }
        private readonly HttpClient HttpClient;

        public CoordinateHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            HttpClient = new HttpClient();
        }

        public async Task<Location> GetCoordinates(Address address)
        {
            var key = Configuration["Geocoding:Apikey"];
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = key };
            var request = await geocoder.GeocodeAsync($"{address.Region} oblast, {address.District} district, {address.Settlement} street {address.Street}, {address.HouseNumber}, Ukraine");
            var coords = request.FirstOrDefault().Coordinates;
            var coords = new Location(50, 50);

            return coords;
        }
    }

}
