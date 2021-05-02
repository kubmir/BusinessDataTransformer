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

            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var result = dataLoader.LoadDataFromFile(desktopFolder + "/Diplomovka_ESF/transformed_10000.csv");
            // var result = dataLoader.LoadDataFromFile(desktopFolder + "/Diplomovka_ESF/owners.csv");

            List<BusinessDataItem> parsedBusinessData = new List<BusinessDataItem>();
            result.ForEach(res => parsedBusinessData.AddRange(dateProcessor.SplitBusinessDataByYear(res)));

            var allLoadedIcos = parsedBusinessData.Select(businessData => businessData.ICO).Distinct().OrderBy(ico => ico).ToList();
            var allIcosWithFinancialData = excelReader.LoadFinancialDataOfCompany(allLoadedIcos, desktopFolder + "/Data_DP/financial_data.xlsx");
            Console.WriteLine($"AllLoadedIcos size {allLoadedIcos.Count} vs. icos with financialData size {allIcosWithFinancialData.Count}");

            // var allIcosWithFinancialData = csvFinancialDataLoader.LoadFinancialDataOfCompany(allLoadedIcos, desktopFolder + "/Data_DP/financial_data_less_detail.csv");
            // Console.WriteLine($"AllLoadedIcos size {allLoadedIcos.Count} vs. icos with financialData from csv size {allIcosWithFinancialData.Count}");
          
            List<CompanyOutputData> ownersInfo = dataAggregator.AggregateDataByCompany(parsedBusinessData);

            dataExporter.ExportDataToCsv(ownersInfo);

            Console.WriteLine("Transformation finished");
        }
    }
}
