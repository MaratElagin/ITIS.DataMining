using Multihash.FrequentItemsets;
using Multihash.FrequentItemsets.Infrastructure.InputOutput;

const int supportMin = 3;
var transactions = CsvReader.FillListFromCsv("transactions.csv");
Processor.Process(supportMin, transactions);