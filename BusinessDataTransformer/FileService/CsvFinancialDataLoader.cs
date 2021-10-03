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
        int i = 0;

        public List<String> LoadFinancialDataOfCompany(List<String> icosInDataset, String pathToFile)
        {
            var allLines = File.ReadAllLines(pathToFile, Encoding.UTF8);
            Console.WriteLine("All lines count " + allLines.Length);


            return allLines
                    .Where(value => {
                        var data = value.Split(';');

                        i++;
                        if (i % 1000 == 0)
                            Console.WriteLine("Processing financial data row " + i);

                        if (data.Length < 3)
                        {
                            return false;
                        }

                        var yearNumber = 0;
                        int.TryParse(data[2], out yearNumber);

                        if (yearNumber > 2011)
                        {
                            return icosInDataset.Contains(value.Split(';')[0]);

                        }

                        return false;
                    })
                    .Select(value => value.Split(';')[0])
                    .Distinct()
                    .ToList();
        }
    }
}
