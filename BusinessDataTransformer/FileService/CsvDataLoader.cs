using System.Linq;
using System.IO;
using BusinessDataTransformer.Model;
using System.Collections.Generic;
using System.Text;

namespace BusinessDataTransformer.FileService
{
    public class CsvDataLoader : IDataLoader
    {

        public List<BusinessDataItem> LoadDataFromFile(string filePath)
            => File.ReadAllLines(filePath, Encoding.UTF8)
                   .Skip(1)
                   .Select(value => BusinessDataItem.FromCsv(value))
                   .ToList();
    }
}
