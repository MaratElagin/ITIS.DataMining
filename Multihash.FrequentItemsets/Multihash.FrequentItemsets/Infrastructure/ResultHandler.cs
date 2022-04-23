namespace Multihash.FrequentItemsets.Infrastructure;

public static class ResultHandler
{
    static readonly Dictionary<int, List<Dictionary<Tuple<int, int>, int>>> Hashes =
        PairsSorter.GetHashes();

    private static Dictionary<string, Dictionary<int, int>> productsId = ProductsHelper.GetProductsIdDictionary();

    static HashSet<Tuple<string, string>> GetFrequentItemsProductNamesTuples()
    {
        HashSet<Tuple<string, string>> productsNamePairs = new();
        var orderedFrequentItemsTuples = GetFrequentItemsIdTuples().OrderByDescending(p => p.Value);
        foreach (var itemsIdTuple in orderedFrequentItemsTuples)
        {
            string firstProductName = "";
            string secondProductName = "";
            foreach (var dict in productsId.Values)
            {
                if (firstProductName == "")
                    firstProductName = GetProductName(itemsIdTuple.Key.Item1);
                else if (secondProductName == "")
                    secondProductName = GetProductName(itemsIdTuple.Key.Item2);
                else
                {
                    productsNamePairs.Add(Tuple.Create(firstProductName, secondProductName));
                    break;
                }
            }
        }

        return productsNamePairs;
    }


    static Dictionary<Tuple<int, int>, int> GetFrequentItemsIdTuples()
    {
        Dictionary<Tuple<int, int>, int> frequentItemsIdTuplesWithCountDict = new();
        foreach (var list in Hashes.Values)
        {
            foreach (var dict in list)
            {
                var dictFirst = dict.First();
                var pair = dictFirst.Key;
                var count = dictFirst.Value;
                frequentItemsIdTuplesWithCountDict.Add(pair, count);
            }
        }

        return frequentItemsIdTuplesWithCountDict;
    }

    static string GetProductName(int id)
    {
        string productName = "";
        foreach (var dict in productsId.Values)
        {
            if (dict.ContainsKey(id))
            {
                productName = productsId.FirstOrDefault(m => m.Value == dict).Key;
                break;
            }
        }

        return productName;
    }

    public static void SaveData()
    {
        Output.SaveDataToExcelFile(ProductsHelper.GetSingleFrequentProductNames(),
            GetFrequentItemsProductNamesTuples());
    }
}