using System;
using System.Globalization;
using BusinessDataTransformer.DateProcessor;
using BusinessDataTransformer.Model;
using NUnit.Framework;

namespace BusinessDataTransformerTests
{
    public class DateProcessorTests
    {
        [Test]
        public void SplitBusinessDataByYears_OneYear()
        {
            var inputBusinessItem = new BusinessDataItem
            {
                ICO = "123",
                Name = "Test company",
                LegalFormOfOwner = "s.r.o",
                CountryOfOwner = "Czech republic",
                OwnerCountrySign = "DOM",
                OwnerType = "FO",
                OwnerShare = "50",
                FromTime = DateTime.ParseExact("06.05.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                ToTime = DateTime.ParseExact("31.08.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                IsValid = "1",
            };

            var dateProcessor = new DateProcessor();
            var result = dateProcessor.SplitBusinessDataByYear(inputBusinessItem);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(inputBusinessItem, result[0]);
        }

        [Test]
        public void SplitBusinessDataByYears_TwoYears()
        {
            var inputBusinessItem = new BusinessDataItem
            {
                ICO = "123",
                Name = "Test company",
                LegalFormOfOwner = "s.r.o",
                CountryOfOwner = "Czech republic",
                OwnerCountrySign = "DOM",
                OwnerType = "FO",
                OwnerShare = "50",
                FromTime = DateTime.ParseExact("01.01.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                ToTime = DateTime.ParseExact("31.12.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                IsValid = "1",
            };

            var dateProcessor = new DateProcessor();
            var result = dateProcessor.SplitBusinessDataByYear(inputBusinessItem);

            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(DateTime.ParseExact("01.01.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture), result[0].FromTime);
            Assert.AreEqual(DateTime.ParseExact("31.12.2020", "dd.MM.yyyy", CultureInfo.CurrentCulture), result[0].ToTime);

            Assert.AreEqual(DateTime.ParseExact("01.01.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture), result[1].FromTime);
            Assert.AreEqual(DateTime.ParseExact("31.12.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture), result[1].ToTime);
        }

        [Test]
        public void SplitBusinessDataByYears_MoreWholeYears()
        {
            var inputBusinessItem = new BusinessDataItem
            {
                ICO = "123",
                Name = "Test company",
                LegalFormOfOwner = "s.r.o",
                CountryOfOwner = "Czech republic",
                OwnerCountrySign = "DOM",
                OwnerType = "FO",
                OwnerShare = "50",
                FromTime = DateTime.ParseExact("01.01.2015", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                ToTime = DateTime.ParseExact("31.12.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                IsValid = "1",
            };

            var dateProcessor = new DateProcessor();
            var result = dateProcessor.SplitBusinessDataByYear(inputBusinessItem);

            Assert.AreEqual(7, result.Count);
        }

        [Test]
        public void SplitBusinessDataByYears_MoreYears()
        {
            var inputBusinessItem = new BusinessDataItem
            {
                ICO = "123",
                Name = "Test company",
                LegalFormOfOwner = "s.r.o",
                CountryOfOwner = "Czech republic",
                OwnerCountrySign = "DOM",
                OwnerType = "FO",
                OwnerShare = "50",
                FromTime = DateTime.ParseExact("01.07.2015", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                ToTime = DateTime.ParseExact("30.09.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                IsValid = "1",
            };

            var dateProcessor = new DateProcessor();
            var result = dateProcessor.SplitBusinessDataByYear(inputBusinessItem);

            Assert.AreEqual(7, result.Count);

            Assert.AreEqual(DateTime.ParseExact("01.07.2015", "dd.MM.yyyy", CultureInfo.CurrentCulture), result[0].FromTime);
            Assert.AreEqual(DateTime.ParseExact("31.12.2015", "dd.MM.yyyy", CultureInfo.CurrentCulture), result[0].ToTime);

            Assert.AreEqual(DateTime.ParseExact("01.01.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture), result[6].FromTime);
            Assert.AreEqual(DateTime.ParseExact("30.09.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture), result[6].ToTime);
        }
    }
}