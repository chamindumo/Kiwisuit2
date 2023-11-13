using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class GoogleGeocodingService
{
    private readonly string apiKey;
    private readonly HttpClient httpClient;

    public GoogleGeocodingService(string apiKey)
    {
        this.apiKey = apiKey;
        this.httpClient = new HttpClient();
    }

    public async Task<Coordinates> GeocodeAsync(string address)
    {
        var encodedAddress = Uri.EscapeDataString(address);
        var apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={apiKey}";

        var response = await httpClient.GetStringAsync(apiUrl);
        var json = JObject.Parse(response);

        if (json["status"].ToString() == "OK")
        {
            var location = json["results"][0]["geometry"]["location"];
            var latitude = location["lat"].Value<double>();
            var longitude = location["lng"].Value<double>();

            return new Coordinates(latitude, longitude);
        }

        // Handle the case where geocoding was not successful
        throw new GeocodingException($"Geocoding failed for address: {address}");
    }
}

public class Coordinates
{
    public double Latitude { get; }
    public double Longitude { get; }

    public Coordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

public class GeocodingException : Exception
{
    public GeocodingException(string message) : base(message)
    {
    }
}
