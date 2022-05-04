using System;
using CBF;

public class Program
{
    static void Main(string[] args)
    {
        string text = "";
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        var filePath = Path.Combine(dir.Parent.Parent.Parent.ToString(), "Chapter11.txt");
        
        using (var sr = new StreamReader(filePath))
            text = sr.ReadToEnd();
        
        char[] separators = {' ', '\n', ',', '.', '-', ':', ';', '?', '!', ')', '('};
        string[] words = text
            .Split(separators,
                StringSplitOptions.RemoveEmptyEntries)
            .Distinct()
            .ToArray();

        var cbf = new Cbf(words.Length, 0.2);
        foreach (var word in words)
            cbf.Add(word);
        
        
        Console.WriteLine($"really {cbf.FindString("really")}");
        Console.WriteLine($"see {cbf.FindString("see")}"); //there is no this word. There is "seen"
        Console.WriteLine($"Tomkins {cbf.FindString("Tomkins")}");//false-positive when 0.2, true when less
        Console.WriteLine($"realy {cbf.FindString("realy")}"); //false-positive when 0.2, true when less
        Console.WriteLine($"hat {cbf.FindString("hat")}");//there is no hat in the text. There is what, that...
        
        Console.WriteLine("--------");
        cbf.Remove("really");
        Console.WriteLine(cbf.FindString("really"));
    }
}