using System;
using System.Collections.Generic;
using System.Linq;

using BusinessDataTransformer.Aggregators;
using BusinessDataTransformer.FileService;
using BusinessDataTransformer.Model;
using BusinessDataTransformer.Processors;

namespace BusinessDataTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataLoader = new CsvDataLoader();
            var dateProcessor = new DateProcessor();
            var dataAggregator = new DataAggregator();
            var dataExporter = new CsvDataExporter();
            var excelReader = new ExcelDataLoader();
            var csvFinancialDataLoader = new CsvFinancialDataLoader();

            Console.WriteLine("Loading data...");
            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            // var result = dataLoader.LoadDataFromFile(desktopFolder + "/Data_DP/complet/Vlastnici_all.csv");
            var result = dataLoader.LoadDataFromFile(desktopFolder + "/Diplomovka_ESF/owners_issue.csv");

            Console.WriteLine("Spliting data by year...");
            List<BusinessDataItem> parsedBusinessData = new List<BusinessDataItem>();
            result.ForEach(res => parsedBusinessData.AddRange(dateProcessor.SplitBusinessDataByYear(res)));

            Console.WriteLine("Distincting ICO...");
            var allLoadedIcos = parsedBusinessData.Select(businessData => businessData.ICO).Distinct().OrderBy(ico => ico).ToList();

            Console.WriteLine("Loading financial data...");
            var companiesFinancialData = excelReader.LoadFinancialDataOfCompany(allLoadedIcos, desktopFolder + "/Data_DP/financial_data.xlsx");

            Console.WriteLine("Aggregating data...");
            List<CompanyOutputData> ownersInfo = dataAggregator.AggregateDataByCompany(parsedBusinessData, companiesFinancialData);

            dataExporter.ExportDataToCsv(ownersInfo);

            Console.WriteLine("Transformation finished");
        }
    }
}
