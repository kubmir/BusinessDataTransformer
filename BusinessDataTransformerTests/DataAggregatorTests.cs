using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessDataTransformer.Aggregators;
using BusinessDataTransformer.Model;
using NUnit.Framework;

namespace BusinessDataTransformerTests
{
    public class DataAggregatorTests
    {
        [Test]
        public void AggregateTwoOwnersOfOneYear()
        {
            var inputBusinessItems = new List<BusinessDataItem> {
                new BusinessDataItem  {
                    OwnerId = "21442131",
                    ICO = "123",
                    Name = "Test company",
                    LegalFormOfOwner = "s.r.o",
                    CountryOfOwner = "Czech republic",
                    OwnerCountrySign = "DOM",
                    OwnerType = "FO",
                    OwnerShare = "50",
                    FromTime = DateTime.ParseExact("01.01.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    ToTime = DateTime.ParseExact("31.12.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    IsValid = "1",
                },
                new BusinessDataItem  {
                    OwnerId = "534523",
                    ICO = "123",
                    Name = "Test company",
                    LegalFormOfOwner = "s.r.o",
                    CountryOfOwner = "USA",
                    OwnerCountrySign = "FOR",
                    OwnerType = "FO",
                    OwnerShare = "50",
                    FromTime = DateTime.ParseExact("01.01.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    ToTime = DateTime.ParseExact("31.12.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    IsValid = "1",
                },
            };

            var czechOwnerInfo = new OwnerInfo
            {
                OwnerId = "21442131",
                LegalFormOfOwner = "s.r.o",
                CountryOfOwner = "Czech republic",
                OwnerCountrySign = "DOM",
                OwnerType = "FO",
                OwnerShare = "50",
                IsValid = "1",
            };

            var usaOwnerInfo = new OwnerInfo
            {
                OwnerId = "534523",
                LegalFormOfOwner = "s.r.o",
                CountryOfOwner = "USA",
                OwnerCountrySign = "FOR",
                OwnerType = "FO",
                OwnerShare = "50",
                IsValid = "1",
            };

            var dataAggregator = new DataAggregator();
            var result = dataAggregator.AggregateDataByCompany(inputBusinessItems);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("123", result[0].ICO);
            Assert.AreEqual("Test company", result[0].Name);
            Assert.AreEqual(1, result[0].OwnersByYears.Keys.Count);

            Assert.IsTrue(result[0].OwnersByYears.ContainsKey(2020));
            Assert.AreEqual(2, result[0].OwnersByYears[2020].Count);

            Assert.AreEqual(czechOwnerInfo.OwnerId, result[0].OwnersByYears[2020][0].OwnerId);
            Assert.AreEqual(usaOwnerInfo.OwnerId, result[0].OwnersByYears[2020][1].OwnerId);
        }

        [Test]
        public void AggregateTwoOwnersOfOneYear_OneOfSecondYear()
        {
            var inputBusinessItems = new List<BusinessDataItem> {
                new BusinessDataItem  {
                    OwnerId = "21442131",
                    ICO = "123",
                    Name = "Test company",
                    LegalFormOfOwner = "s.r.o",
                    CountryOfOwner = "Czech republic",
                    OwnerCountrySign = "DOM",
                    OwnerType = "FO",
                    OwnerShare = "50",
                    FromTime = DateTime.ParseExact("01.01.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    ToTime = DateTime.ParseExact("31.12.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    IsValid = "1",
                },
                new BusinessDataItem  {
                    OwnerId = "21442131",
                    ICO = "123",
                    Name = "Test company",
                    LegalFormOfOwner = "s.r.o",
                    CountryOfOwner = "Czech republic",
                    OwnerCountrySign = "DOM",
                    OwnerType = "FO",
                    OwnerShare = "50",
                    FromTime = DateTime.ParseExact("01.01.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    ToTime = DateTime.ParseExact("31.12.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    IsValid = "1",
                },
                new BusinessDataItem  {
                    OwnerId = "534523",
                    ICO = "123",
                    Name = "Test company",
                    LegalFormOfOwner = "s.r.o",
                    CountryOfOwner = "USA",
                    OwnerCountrySign = "FOR",
                    OwnerType = "FO",
                    OwnerShare = "50",
                    FromTime = DateTime.ParseExact("01.01.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    ToTime = DateTime.ParseExact("31.12.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                    IsValid = "1",
                },
            };

            var czechOwnerInfo = new OwnerInfo
            {
                OwnerId = "21442131",
                LegalFormOfOwner = "s.r.o",
                CountryOfOwner = "Czech republic",
                OwnerCountrySign = "DOM",
                OwnerType = "FO",
                OwnerShare = "50",
                IsValid = "1",
            };

            var usaOwnerInfo = new OwnerInfo
            {
                OwnerId = "534523",
                LegalFormOfOwner = "s.r.o",
                CountryOfOwner = "USA",
                OwnerCountrySign = "FOR",
                OwnerType = "FO",
                OwnerShare = "50",
                IsValid = "1",
            };

            var dataAggregator = new DataAggregator();
            var result = dataAggregator.AggregateDataByCompany(inputBusinessItems);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("123", result[0].ICO);
            Assert.AreEqual("Test company", result[0].Name);
            Assert.AreEqual(2, result[0].OwnersByYears.Keys.Count);

            Assert.IsTrue(result[0].OwnersByYears.ContainsKey(2020));
            Assert.IsTrue(result[0].OwnersByYears.ContainsKey(2021));

            Assert.AreEqual(2, result[0].OwnersByYears[2020].Count);
            Assert.AreEqual(1, result[0].OwnersByYears[2021].Count);

            Assert.AreEqual(czechOwnerInfo.OwnerId, result[0].OwnersByYears[2020][0].OwnerId);
            Assert.AreEqual(usaOwnerInfo.OwnerId, result[0].OwnersByYears[2020][1].OwnerId);
            Assert.AreEqual(czechOwnerInfo.OwnerId, result[0].OwnersByYears[2021][0].OwnerId);
        }
    }
}
