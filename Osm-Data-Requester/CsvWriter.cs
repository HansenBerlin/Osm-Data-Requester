namespace Osm_Data_Requester;

public class CsvWriter
{
    
    public string[] GetFirstColumnValuesFromCsv(string filePath)
    {
        using StreamReader sr = new StreamReader(filePath);
        string fileContents = sr.ReadToEnd();
        string[] rows = fileContents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] columnValues = new string[rows.Length];
        for (int i = 1; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split(',');
            columnValues[i] = columns[1];
        }

        return columnValues;
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