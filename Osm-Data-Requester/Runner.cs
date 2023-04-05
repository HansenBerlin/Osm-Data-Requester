namespace Osm_Data_Requester;

public class Runner
{
    private readonly Requester _requester;
    private readonly CsvWriter _writer;

    public Runner(Requester requester, CsvWriter writer)
    {
        _requester = requester;
        _writer = writer;
    }

    public async Task Run(List<string> csvContent, int startIndex, int endIndex, string outputPath, int throttle)
    {
        _writer.InitCsv(outputPath);
        if (csvContent.Count > 0)
        {
            csvContent.RemoveAt(0);
        }
        int length = csvContent.Count;
        endIndex = endIndex == -1 ? length : endIndex - 1 > length ? length : endIndex - 1;
        Console.WriteLine($"Starting requests every {throttle}ms, beginning at index {startIndex} " +
                          $"till {endIndex + 1}. Length of file: {length}.");
        for (int i = startIndex - 1; i < endIndex; i++)
        {
            var value = csvContent[i];
            await Task.Delay(throttle);
            var location = await _requester.Get(value, i);
            _writer.AppendLocationToCsv(location);
        }
    }
}