using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton()<IExampleTransientService, ExampleTransientService>();
        
    })
    .Build();

var requester = new R

const string filePath = @"data\immo_data_cleared_reduced.csv";
const string baseUrl = "https://nominatim.openstreetmap.org/search?q=";
const string urlEnd = "&format=json&polygon=1&addressdetails=1";


HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

List<Location> locations = new();
string[] columnValues = GetFirstColumnValuesFromCsv(filePath);


for (int i = 1; i < 600; i++)
{
    var value = columnValues[i];
    string url = $"{baseUrl}{value}{urlEnd}";
    await Task.Delay(1000);
    Location location = GetLocationFromApi(client, url, i);
    locations.Add(location);
}

WriteLocationsToCsv(locations, @"data\immodatanew.csv");




static Location GetLocationFromApi(HttpClient client, string url, int increment)
{
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    using var request = new HttpRequestMessage(HttpMethod.Get, url);
    using var response = client.SendAsync(request).Result;
    
    
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
            json = response.Content.ReadAsStringAsync().Result;
            var result = JArray.Parse(json)[0];
            var lat = result.Value<string>("lat");
            var lon = result.Value<string>("lon");
            return new Location { Lat = lat, Lon = lon };
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


