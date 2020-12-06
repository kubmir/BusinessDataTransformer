using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var result = dataLoader.LoadDataFromFile(desktopFolder + "/Diplomovka_ESF/10000_init.csv");

            List<BusinessDataItem> parsedBusinessData = new List<BusinessDataItem>();
            result.ForEach(res => parsedBusinessData.AddRange(dateProcessor.SplitBusinessDataByYear(res)));

            List<CompanyOutputData> ownersInfo = dataAggregator.AggregateDataByCompany(parsedBusinessData);

            dataExporter.ExportDataToCsv(ownersInfo);

            Console.WriteLine("Transformation finished");
        }
    }
}
