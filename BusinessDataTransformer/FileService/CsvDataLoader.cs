using System.Linq;
using System.IO;
using BusinessDataTransformer.Model;
using System.Collections.Generic;

namespace BusinessDataTransformer.FileService
{
    public class CsvDataLoader : IDataLoader
    {

        public List<BusinessDataItem> LoadDataFromFile(string filePath)
            => File.ReadAllLines(filePath)
                   .Skip(1)
                   .Select(value => BusinessDataItem.FromCsv(value))
                   .ToList();
    }
}
