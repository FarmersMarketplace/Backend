using Geocoding;
using Geocoding.Google;
using Microsoft.Extensions.Configuration;
using Address = FarmersMarketplace.Domain.Address;

namespace FarmersMarketplace.Application.Helpers
{
    public class CoordinateHelper
    {
        protected IConfiguration Configuration { get; set; }

        public CoordinateHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<Location> GetCoordinates(Address address)
        {
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = Configuration["Geocoding:Apikey"] };
            var request = await geocoder.GeocodeAsync($"{address.Region} oblast, {address.District} district, {address.Settlement} street {address.Street}, {address.HouseNumber}, Ukraine");
            var coords = request.FirstOrDefault().Coordinates;
            return coords;
        }
    }

}
