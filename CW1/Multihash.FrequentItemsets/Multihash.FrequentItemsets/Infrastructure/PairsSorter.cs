namespace Multihash.FrequentItemsets.Infrastructure;

public static class PairsSorter
{
    private static readonly Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> Hashes =
        HashLogic.GetHashes1();

    private static readonly Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> Hashes2 =
        HashLogic.GetHashes2();

    private static HashSet<Tuple<int, int>> hashSet = new();

    public static HashSet<Tuple<int, int>> GetTupleOfProductIdsToRemoveWhereSumLessThanSupportMin(
        Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> hashes, int supportMin)
    {
        HashSet<Tuple<int, int>> tuplesToRemove = new();
        foreach (var list in hashes.Values)
        {
            var pairsList = new List<Tuple<int, int>>();
            var sum = 0;
            foreach (var dict in list)
            {
                var d = dict.First();
                pairsList.Add(d.Key);
                sum += d.Value;
            }

            if (sum < supportMin)
            {
                foreach (var pair in pairsList)
                    tuplesToRemove.Add(pair);
            }
        }

        return tuplesToRemove;
    }

    public static void RemovePairsWhereSumLessThanSupportMin(int supportMin)
    {
        hashSet = GetTupleOfProductIdsToRemoveWhereSumLessThanSupportMin(Hashes, supportMin);
        var hashSet2 = GetTupleOfProductIdsToRemoveWhereSumLessThanSupportMin(Hashes2, supportMin);
        hashSet.UnionWith(hashSet2);
        RemovePairs();
    }

    private static void RemovePairs()
    {
        foreach (var list in Hashes.Values)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var pair = list[i].First().Key;
                if (hashSet.Contains(pair))
                    list.Remove(list[i]);
            }

            if (list.Count == 0)
            {
                var key = Hashes.FirstOrDefault(m => m.Value == list).Key;
                Hashes.Remove(key);
            }
        }
    }

    public static void RemovePairsWithNonFrequentProducts()
    {
        var nonFrequentHashSet = ProductsHelper.GetNonFrequentHashSet();
        foreach (var list in Hashes.Values)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var pair = list[i].First().Key;
                if (nonFrequentHashSet.Contains(pair.Item1)
                    || nonFrequentHashSet.Contains(pair.Item2))
                    list.Remove(list[i]);
            }
        }
    }

    public static Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> GetHashes() => Hashes;
}