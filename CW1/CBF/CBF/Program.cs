using System;
using CBF;

public class Program
{
    static void Main(string[] args)
    {
        string text = "";
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.ToString();
        var filePath = Path.Combine(dir, "text.txt");
        
        using (var sr = new StreamReader(filePath))
            text = sr.ReadToEnd();
        
        char[] separators = {' ', '\n','\r', '-', ',', '.', ':', ';', '?', '!', ')', '('};
        string[] words = text
            .Split(separators,
                StringSplitOptions.RemoveEmptyEntries)
            .Distinct()
            .ToArray();

        var cbf = new Cbf(words.Length, 0.1);
        foreach (var word in words)
            cbf.Add(word);

        Console.WriteLine(cbf.FindString("улыбки"));
        
        var filterFilePath = Path.Combine(dir, "filter.txt");
        cbf.WriteFilterToFile(filterFilePath);
    }
}