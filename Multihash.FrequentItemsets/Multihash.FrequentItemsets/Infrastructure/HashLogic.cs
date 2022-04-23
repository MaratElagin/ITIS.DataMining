namespace Multihash.FrequentItemsets.Infrastructure;

public static class HashLogic
{
    private static readonly int K = ProductsHelper.GetProductsCount();

    delegate int Hash(int i, int j);

    private static readonly Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> Hashes1 = new();
    private static readonly Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> Hashes2 = new();
    static int Hash1(int i, int j) => (i + j) % K;

    static int Hash2(int i, int j) => (i + 3 * j) % K;

    static void FillHashDictionary(Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> hashes,
        Tuple<int, int> pair, Hash hashFunction)
    {
        var hash = hashFunction(pair.Item1, pair.Item2);
        if (!hashes.ContainsKey(hash))
        {
            var pairDict = new Dictionary<Tuple<int, int>, int> {{pair, 1}};
            hashes.Add(hash, new List<Dictionary<Tuple<int, int>, int>> {pairDict});
        }
        else
        {
            var currentPair = hashes[hash].FirstOrDefault(d => d.ContainsKey(pair));
            if (currentPair == null)
            {
                var pairDict = new Dictionary<Tuple<int, int>, int> {{pair, 1}};
                hashes[hash].Add(pairDict);
            }
            else
                currentPair[pair]++;
        }
    }

    public static void FillHashDictionaries()
    {
        var buckets = ProductsHelper.GetBucketsDictionary();
        foreach (var bucket in buckets)
        {
            var productsList = bucket.Value.ToArray();
            var n = productsList.Length;
            for (var i = 0; i < n - 1; i++)
            {
                for (var j = i + 1; j < n; j++)
                {
                    var pair = Tuple.Create(productsList[i], productsList[j]);
                    FillHashDictionary(Hashes1, pair, Hash1);
                    FillHashDictionary(Hashes2, pair, Hash2);
                }
            }
        }
    }

    public static Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> GetHashes1() => Hashes1;
    public static Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> GetHashes2() => Hashes2;
}