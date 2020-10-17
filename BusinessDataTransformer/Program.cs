using System;
using System.Collections.Generic;
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

            var result = dataLoader.LoadDataFromFile("/Users/miroslavkubus/Desktop/owners.csv");

            List<BusinessDataItem> parsedBusinessData = new List<BusinessDataItem>();
            result.ForEach(res => parsedBusinessData.AddRange(dateProcessor.SplitBusinessDataByYear(res)));

            List<CompanyOutputData> ownersInfo = dataAggregator.AggregateDataByCompany(parsedBusinessData);

            ownersInfo.ForEach(res => Console.WriteLine(res));
        }
    }
}
