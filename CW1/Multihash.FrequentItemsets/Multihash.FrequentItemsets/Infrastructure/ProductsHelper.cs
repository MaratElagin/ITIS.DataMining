namespace Multihash.FrequentItemsets.Infrastructure;

public static class ProductsHelper
{
    private static Dictionary<string, List<int>> buckets = new();
    private static Dictionary<string, Dictionary<int, int>> productsId = new();
    private static HashSet<int> nonFrequentProductsId = new();


    private static void AddProductToProductIdsDict(string product, ref int id)
    {
        if (!productsId.ContainsKey(product))
        {
            var dict = new Dictionary<int, int> {{id++, 1}};
            productsId.Add(product, dict);
        }
        else
        {
            productsId.TryGetValue(product, out var d);
            var di = d.First();
            var count = di.Value + 1;
            var existingId = di.Key;
            d[existingId] = count;
            productsId[product] = d;
        }
    }

    private static void AddProductIdToBucket(string product, string bucketId)
    {
        productsId.TryGetValue(product, out var productIdCount);
        var productId = productIdCount.First().Key;
        if (!buckets.ContainsKey(bucketId))
            buckets.Add(bucketId, new List<int> {productId});
        else buckets[bucketId].Add(productId);
    }

    /// <summary>
    /// Match productName with id and number of occurrences.
    /// Match bucketId with list of products ids
    /// </summary>
    /// <param name="transactions"></param>
    public static void FillBucketsWithProductIdsList(List<string> transactions)
    {
        //Dictionary<string, Dictionary<int, int>> productsId = new();
        var id = 1;
        foreach (var line in transactions)
        {
            var data = line.Split(";");
            var product = data[0];
            var bucketId = data[1];

            AddProductToProductIdsDict(product, ref id);
            AddProductIdToBucket(product, bucketId);
        }
    }

    /// <summary>
    /// Get products ids that have number of occurrences less than the support minimum level 
    /// </summary>
    /// <returns>HashSet with products ids</returns>
    public static void GetNonFrequentProductsIds(int supportMin)
    {
        var orderedByCountProductId = productsId.OrderByDescending(p => p.Value.First().Value);
        foreach (var productId in orderedByCountProductId)
        {
            var productCount = productId.Value.First().Value;
            if (productCount < supportMin)
            {
                var notFrequentProductId = productId.Value.First().Key;
                nonFrequentProductsId.Add(notFrequentProductId);
            }
        }
    }

    /// <summary>
    /// Get product name list with number of occurrences not less than the support minimum level
    /// </summary>
    /// <returns></returns>
    public static List<string> GetSingleFrequentProductNames()
    {
        List<string> singleFrequentProducts = new();
        foreach (var dict in productsId.Values)
        {
            if (!nonFrequentProductsId.Contains(dict.First().Key))
            {
                var productName = productsId.FirstOrDefault(m => m.Value == dict).Key;
                singleFrequentProducts.Add(productName);
            }
        }

        return singleFrequentProducts;
    }

    public static Dictionary<string, List<int>> GetBucketsDictionary() => buckets;

    public static Dictionary<string, Dictionary<int, int>> GetProductsIdDictionary() => productsId;
    public static int GetProductsCount() => productsId.Count;
    public static HashSet<int> GetNonFrequentHashSet() => nonFrequentProductsId;
}