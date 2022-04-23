namespace Multihash.FrequentItemsets.Infrastructure.InputOutput;

public static class CsvReader
{
    public static List<string> FillListFromCsv(string csvFileName)
    {
        var list = new List<string>();
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        var pathToFile = Path.Combine(dir.Parent.Parent.Parent.ToString(), "inputs",  csvFileName);
        var lines = File.ReadLines(pathToFile).Skip(1).ToArray();
        foreach (var line in lines)
            list.Add(line);
        return list;
    }
}