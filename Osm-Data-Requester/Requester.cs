using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Osm_Data_Requester;

public class Requester
{
    private const string BaseUrl = "https://nominatim.openstreetmap.org/search?q=";
    private const string UrlEnd = "&format=json&polygon=1&addressdetails=1";

    private readonly HttpClient _client;

    public Requester(HttpClient client)
    {
        _client = client;
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        _client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
    }

    public async Task <Location> Get(string endpoint, int increment)
    {
        string url = $"{BaseUrl}{endpoint}{UrlEnd}";

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await _client.SendAsync(request);
        
        
        TimeSpan ts = stopWatch.Elapsed;

        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine($"Abfrage {increment}: {elapsedTime}");
        string json = "";
        try
        {
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                var result = JArray.Parse(json)[0];
                var lat = result.Value<string>("lat");
                var lon = result.Value<string>("lon");
                return new Location { Lat = lat ?? string.Empty, Lon = lon ?? string.Empty };
            }

            return new Location();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine(json);
            Console.WriteLine(url);
            return new Location();
        }
    }
}