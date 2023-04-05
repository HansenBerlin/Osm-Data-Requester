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

    public async Task Run(string[] csvContent, int startIndex, int endIndex, string outputPath, int throttle)
    {
        _writer.InitCsv(outputPath);
        int length = csvContent.Length;
        endIndex = endIndex == -1 ? length : endIndex;
        Console.WriteLine($"Starting requests every {throttle}ms, beginning at index {startIndex + 1} " +
                          $"till {endIndex + 1}. Length of file: {length - 1}.");
        for (int i = 1 + startIndex; i < endIndex + 1; i++)
        {
            if (i == length + 1)
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