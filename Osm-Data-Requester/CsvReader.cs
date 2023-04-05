namespace Osm_Data_Requester;

public class CsvReader
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
}