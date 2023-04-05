﻿namespace Osm_Data_Requester;

public class Runner
{
    private readonly Requester _requester;
    private readonly CsvWriter _writer;

    public Runner(Requester requester, CsvWriter writer)
    {
        _requester = requester;
        _writer = writer;
    }

    public async Task Run(string[] csvContent, int startIndex, int endIndex, string outputPath, int throttle)
    {
        _writer.InitCsv(outputPath);
        int length = csvContent.Length;
        endIndex = endIndex == -1 ? length : endIndex;
        for (int i = 1 + startIndex; i < endIndex; i++)
        {
            if (i - 1 == length)
            {
                break;
            }
            var value = csvContent[i];
            await Task.Delay(throttle);
            var location = await _requester.Get(value, i);
            _writer.AppendLocationToCsv(location);
        }
    }
}