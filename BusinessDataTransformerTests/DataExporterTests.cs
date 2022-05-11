using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessDataTransformer.FileService;
using BusinessDataTransformer.Model;
using NUnit.Framework;

namespace BusinessDataTransformerTests
{
    public class DataExporter
    {
        [Test]
        public void ExportDataAsCsv_TwoYears_DifferentCountOfOwners()
        {
            CompanyOutputData companyOutputData = new CompanyOutputData
            {
                ICO = "123",
                Name = "Test company",
                OwnersByYears = new Dictionary<int, List<OwnerInfo>>()
                {
                    { 2011, new List<OwnerInfo>
                        {
                            new OwnerInfo
                            {
                                LegalFormOfOwner = "s.r.o",
                                CountryOfOwner = "Czech republic",
                                OwnerCountrySign = "DOM",
                                OwnerType = "FO",
                                OwnerShare = 51,
                                FromTime = DateTime.ParseExact("01.01.2011", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                                ToTime = DateTime.ParseExact("31.12.2011", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                                IsValid = "1",
                            },
                            new OwnerInfo
                            {
                                LegalFormOfOwner = "s.r.o",
                                CountryOfOwner = "USA",
                                OwnerCountrySign = "FOR",
                                OwnerType = "FO",
                                OwnerShare = 49,
                                FromTime = DateTime.ParseExact("01.01.2011", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                                ToTime = DateTime.ParseExact("31.12.2011", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                                IsValid = "1",
                            }
                        }
                    },
                    { 2012, new List<OwnerInfo>
                        {
                            new OwnerInfo
                            {
                                LegalFormOfOwner = "s.r.o",
                                CountryOfOwner = "Czech republic",
                                OwnerCountrySign = "DOM",
                                OwnerType = "FO",
                                OwnerShare = 100,
                                FromTime = DateTime.ParseExact("01.01.2012", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                                ToTime = DateTime.ParseExact("31.12.2012", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                                IsValid = "1",
                            },
                        }
                    }
                },
                FinancialResults = new FinancialResultsDataItem
                {
                    ICO = "123",
                    Roa2010 = 1,
                    Roa2011 = 2,
                    Roa2012 = 3,
                    Roa2013 = 4,
                    Roa2014 = 5,
                    Roe2010 = 1,
                    Roe2011 = 2,
                    Roe2012 = 3,
                    Roe2013 = 4,
                    Roe2014 = 5,
                }
            };

            var dataExporter = new CsvDataExporter();

            var result = dataExporter.TransformCompanyDataToCsvString(companyOutputData);

            var expectedResult = "123;Test company;s.r.o;Czech republic;DOM;100;FO;;;;;;;;;;;0;0;0;3.0000;3.0000;";

            Console.WriteLine(expectedResult);
            Console.WriteLine(result);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
