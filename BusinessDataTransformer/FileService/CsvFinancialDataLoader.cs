using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BusinessDataTransformer.FileService
{
    /**
     * On 200 samples is rate of companies with financial data 126:95
     * On 10000 samples is rate of companies with financial data 5357:3187
     */
    public class CsvFinancialDataLoader
    {
        public List<String> LoadFinancialDataOfCompany(List<String> icosInDataset, String pathToFile)
        {
            return File.ReadAllLines(pathToFile, Encoding.UTF8)
                       .Where(value => icosInDataset.Contains(value.Split(';')[0]))
                       .Select(value => value.Split(';')[0])
                       .Distinct()
                       .ToList();
        }
    }
}
