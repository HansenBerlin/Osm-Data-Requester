using Osm_Data_Requester;

class Program
{
    async static Task Main(string[] args)
    {
        string inputPath = "";
        string outputPath = "";
        int startIndex = 0;
        int endIndex = -1;
        try
        {
            inputPath = args[0];
            outputPath = args[1];
            if (int.TryParse(args[2], out startIndex)) { }
            else
            {
                startIndex = 0;
            }
            if (int.TryParse(args[3], out endIndex)) { }
        }
        catch (Exception e)
        {
            Console.WriteLine("Wrong number of arguments provided. Please provide at least an " +
                              "input and outputpath. Additionally you can add an index to start " +
                              "the requests at and an index to end the requests at. When no end " +
                              "index is provided the whole list will be parsed.");
            throw;
        }
        var httpClient = new HttpClient();
        var requester = new Requester(httpClient);
        var csvWriter = new CsvWriter();
        var csvReader = new CsvReader();
        string[] csvContent = csvReader.GetFirstColumnValuesFromCsv(inputPath);
        var runner = new Runner(requester, csvWriter);
        await runner.Run(csvContent, startIndex, endIndex, outputPath);
    }
}


