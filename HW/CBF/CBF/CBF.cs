namespace CBF;

public class Cbf
{
    //bloom filter array
    private readonly uint[] _m;
    private int SizeM { get; }

    //optimal number of hashing functions
    private int _k { get; }
    private HashFunction[] _hashes { get; set; }

    public Cbf(int sizeN, double falsePositive)
    {
        (SizeM, _k) = GetOptimalFilterParameters(sizeN, falsePositive);
        _m = new uint[SizeM];
        _hashes = new HashFunction[_k];
        for (var i = 0; i < _k; i++)
            _hashes[i] = new HashFunction(_k);
    }

    private static (int, int) GetOptimalFilterParameters(int sizeN, double falsePositive)
    {
        double ln2 = Math.Log(2);
        var sizeM = (-1 * sizeN * (Math.Log(falsePositive) / (ln2 * ln2)));
        var hashesAmount = sizeM / sizeN * ln2;
        return ((int) Math.Ceiling(sizeM), (int) Math.Ceiling(hashesAmount));
    }

    public void Add(string s)
    {
        for (int i = 0; i < _k; i++)
            _m[_hashes[i].Hash(s) % SizeM] += 1;
    }

    public void Remove(string s)
    {
        for (int i = 0; i < _k; i++)
            _m[_hashes[i].Hash(s) % SizeM] -= 1;
    }

    public bool FindString(string s)
    {
        var isFound = true;
        for (int i = 0; i < _k; i++)
            if (_m[_hashes[i].Hash(s) % SizeM] <= 0)
                isFound = false;

        return isFound;
    }
}