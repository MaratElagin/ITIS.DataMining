using Multihash.FrequentItemsets.Infrastructure;

namespace Multihash.FrequentItemsets;

public static class Processor
{
    public static void Process(int supportMin, List<string> transactions)
    {
        ProductsHelper.FillBucketsWithProductIdsList(transactions);
        ProductsHelper.GetNonFrequentProductsIds(supportMin);
        ProductsHelper.GetSingleFrequentProductNames();
        HashLogic.FillHashDictionaries();
        PairsSorter.RemovePairsWhereSumLessThanSupportMin(supportMin);
        PairsSorter.RemovePairsWithNonFrequentProducts();
        ResultHandler.SaveData();
    }
}