namespace Osm_Data_Requester;

public class CsvWriter
{
    
    private const string CsvHeader = "id,lat,lon";
    private string _outputPath = "";

    public void InitCsv(string outputPath)
    {
        _outputPath = outputPath;
        if (!File.Exists(outputPath))
        {
            using var writer = new StreamWriter(outputPath);
            writer.WriteLine(CsvHeader);
        }
    }
    
    
    public void AppendLocationToCsv(Location location)
    {
        using var writer = new StreamWriter(_outputPath, true);
        var csvLine = $"{GetLastIdInCsv(_outputPath) + 1},{location.Lat},{location.Lon}";
        writer.WriteLine(csvLine);
    }

    private int GetLastIdInCsv(string outputPath)
    {
        var lastLine = File.ReadLines(outputPath).LastOrDefault();
        if (lastLine == null || !int.TryParse(lastLine.Split(',')[0], out int lastId))
        {
            lastId = 0;
        }
        return lastId;
    }
    
    public void WriteLocationsToCsv(IEnumerable<Location> locations, string outputPath)
    {
        var csvLines = locations.Select(l => $"{l.Lat},{l.Lon}");

        using var writer = new StreamWriter(outputPath);
        writer.WriteLine("id,lat,lon"); // Write header row
        for (int i = 0; i < csvLines.Count(); i++)
        {
            writer.WriteLine($"{i},{csvLines.ElementAt(i)}");
        }
    }
}