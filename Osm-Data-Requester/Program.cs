﻿namespace Osm_Data_Requester;

class Program
{
    static async Task Main(string[] args)
    {
        string inputPath = "";
        string outputPath = "";
        int startIndex = 0;
        int endIndex = -1;
        int throttle = 1000;
        try
        {
            inputPath = args[0];
            outputPath = args[1];
        }
        catch (Exception e)
        {
            Console.WriteLine("Wrong number of arguments provided. Please provide at least an " +
                              "input and outputpath. Additionally you can add an index to start " +
                              "the requests at and an index to end the requests at. When no end " +
                              "index is provided the whole list will be parsed. As fifth argument you" +
                              "can add a throttle in ms for the requests.");
            throw;
        }

        switch (args.Length)
        {
            case 3:
                int.TryParse(args[2], out startIndex);
                break;
            case 4:
                int.TryParse(args[2], out startIndex);
                int.TryParse(args[3], out endIndex);
                break;
            case 5:
                int.TryParse(args[2], out startIndex);
                int.TryParse(args[3], out endIndex);
                int.TryParse(args[4], out throttle);
                break;
        }
        
        var httpClient = new HttpClient();
        var requester = new Requester(httpClient);
        var csvWriter = new CsvWriter();
        var csvReader = new CsvReader();
        string[] csvContent = csvReader.GetFirstColumnValuesFromCsv(inputPath);
        var runner = new Runner(requester, csvWriter);
        await runner.Run(csvContent.ToList(), startIndex, endIndex, outputPath, throttle);
    }
}