using System.Collections.Generic;
using BusinessDataTransformer.Model;

namespace BusinessDataTransformer.FileService
{
    public interface IDataLoader
    {
        public List<BusinessDataItem> LoadDataFromFile(string filePath);
    }
}
