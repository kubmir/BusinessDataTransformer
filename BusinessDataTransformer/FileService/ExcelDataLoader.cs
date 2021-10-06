using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using BusinessDataTransformer.Model;

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

        public List<FinancialResultsDataItem> LoadFinancialDataOfCompany(List<String> icosInDataset, String pathToFile)
        {
            var financialData = new List<FinancialResultsDataItem>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(pathToFile)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var colCount = worksheet.Dimension.End.Column;

                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                {
                    if (row % 1000 == 0) {
                        Console.WriteLine("Processing row" + row);
                    }

                    var currentIco = worksheet.Cells[row, 1].Value.ToString();
                    var currentIcoInDataSetIndex = icosInDataset.IndexOf(currentIco);
                  
                    if (currentIcoInDataSetIndex != -1)
                    {
                        icosInDataset.RemoveAt(currentIcoInDataSetIndex);
                        financialData.Add(FinancialResultsDataItem.FromExcel(worksheet.Cells[row, 1, row, colCount].ToText()));
                    }
                }

            }

            return financialData;
        }
    }
}
