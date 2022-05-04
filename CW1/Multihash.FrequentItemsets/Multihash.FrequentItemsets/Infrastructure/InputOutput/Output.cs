using Ganss.Excel;

namespace Multihash.FrequentItemsets.Infrastructure;

public static class Output
{
    private static ExcelMapper _excelMapper = new();
    private static string _filePath;

    public static void SaveDataToExcelFile(List<string> singletons, HashSet<Tuple<string, string>> doubletons)
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
        _filePath = Path.Combine(directory.ToString(), "Result", "frequentItemsets.xlsx");

        WriteSingletonsToExcel(singletons, "singletons");

        WriteDoubletonsToExcel(doubletons, "doubletons");
    }

    private static void WriteSingletonsToExcel(List<string> singletons, string listName)
    {
        List<Singletons> data = new();
        foreach (var singleton in singletons)
        {
            var result = new Singletons
            {
                Singleton = singleton
            };
            data.Add(result);
        }

        _excelMapper.Save(_filePath, data, listName, true);
    }

    private static void WriteDoubletonsToExcel(HashSet<Tuple<string, string>> doubletons, string listName)
    {
        List<Doubletons> data = new();
        foreach (var doubleton in doubletons)
        {
            var result = new Doubletons
            {
                Doubleton1 = doubleton.Item1,
                Doubleton2 = doubleton.Item2
            };
            data.Add(result);
        }

        _excelMapper.Save(_filePath, data, listName, true);
    }
}

public class Singletons
{
    public string Singleton { get; set; }
}

public class Doubletons
{
    public string Doubleton1 { get; set; }
    public string Doubleton2 { get; set; }
}