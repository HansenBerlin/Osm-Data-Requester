using Osm_Data_Requester;

var httpClient = new HttpClient();
var requester = new Requester(httpClient);
var csvWriter = new CsvWriter();
var csvReader = new CsvReader();
string[] csvContent = csvReader.GetFirstColumnValuesFromCsv(@"data/immo_data_cleared_reduced.csv");
var runner = new Runner(requester, csvWriter);
await runner.Run(csvContent, 0, 100, @"data/newlocations.csv");

