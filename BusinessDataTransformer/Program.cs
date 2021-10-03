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
            var result = dataLoader.LoadDataFromFile(desktopFolder + "/Data_DP/complet/Vlastnici_all.csv");
            // var result = dataLoader.LoadDataFromFile(desktopFolder + "/Diplomovka_ESF/owners.csv");

            Console.WriteLine("Spliting data by year...");
            List<BusinessDataItem> parsedBusinessData = new List<BusinessDataItem>();
            result.ForEach(res => parsedBusinessData.AddRange(dateProcessor.SplitBusinessDataByYear(res)));

            Console.WriteLine("Distincting ICO...");
            var allLoadedIcos = parsedBusinessData.Select(businessData => businessData.ICO).Distinct().OrderBy(ico => ico).ToList();

            Console.WriteLine("Loading financial data...");
            var allIcosWithFinancialData = excelReader.LoadFinancialDataOfCompany(allLoadedIcos, desktopFolder + "/Data_DP/financial_data.xlsx");
            Console.WriteLine($"AllLoadedIcos size {allLoadedIcos.Count} vs. icos with financialData size {allIcosWithFinancialData.Count}");

            // Worse than financial_data.xlsx
            // var allIcosWithFinancialData = csvFinancialDataLoader.LoadFinancialDataOfCompany(allLoadedIcos, desktopFolder + "/Data_DP/financial_data_less_detail.csv");
            // Console.WriteLine($"AllLoadedIcos size {allLoadedIcos.Count} vs. icos with financialData from csv size {allIcosWithFinancialData.Count}");

            List<CompanyOutputData> ownersInfo = dataAggregator.AggregateDataByCompany(parsedBusinessData);

            dataExporter.ExportDataToCsv(ownersInfo);

            Console.WriteLine("Transformation finished");
        }
    }
}
