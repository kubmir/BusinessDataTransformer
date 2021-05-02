using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Linq;

namespace BusinessDataTransformer.FileService
{
    /**
     * On 200 samples is rate of companies with financial data 126:92
     * On 10000 samples is rate of companies with financial data 5357:3151
     */
    public class ExcelDataLoader
    {
        public ExcelDataLoader()
        {

        }

        public List<String> LoadFinancialDataOfCompany(List<String> icosInDataset, String pathToFile)
        {
            var companiesToReturn = new List<string>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(pathToFile)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                {
                    if (row % 1000 == 0) {
                        Console.WriteLine("Processing row" + row);
                    }

                    if (icosInDataset.Contains(worksheet.Cells[row, 1].Value.ToString()))
                    {
                        companiesToReturn.Add(worksheet.Cells[row, 1].Value.ToString());
                    }
                }

            }

            return companiesToReturn.Distinct().OrderBy(val => val).ToList();
        }
    }
}
