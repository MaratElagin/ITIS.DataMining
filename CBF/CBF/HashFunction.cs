namespace CBF;

public class HashFunction
{
    private int seed = 0;

    public HashFunction(int k)
    {
        var rnd = new Random();
        seed = (int)Math.Floor(rnd.NextDouble() * k);
    }

    public uint Hash(string str)
    {
        uint result = 1;
        for (var i = 0; i < str.Length;i++)
            result = (uint)(seed * result + str[i]);
        return result;
    }
}