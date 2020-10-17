using System;
using BusinessDataTransformer.FileService;

namespace BusinessDataTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataLoader = new CsvDataLoader();
            var result = dataLoader.LoadDataFromFile("/Users/miroslavkubus/Desktop/owners.csv");

            result.ForEach(res => Console.WriteLine(res));
        }
    }
}
