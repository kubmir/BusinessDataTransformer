using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessDataTransformer.Model;

namespace BusinessDataTransformer.Processors
{
    public class DateProcessor
    {
        public List<BusinessDataItem> SplitBusinessDataByYear(BusinessDataItem businessDataItem)
        {
            var startYear = businessDataItem.FromTime.Year;
            var endYear = businessDataItem.ToTime.Year;

            if (startYear == endYear)
            {
                return new List<BusinessDataItem>() { businessDataItem };
            }

            var resultList = new List<BusinessDataItem>();

            for (int i = startYear; i <= endYear; i++)
            {
                resultList.Add(
                    new BusinessDataItem
                    {
                        ICO = businessDataItem.ICO,
                        Name = businessDataItem.Name,
                        OwnerCountrySign = businessDataItem.OwnerCountrySign,
                        OwnerShare = businessDataItem.OwnerShare,
                        CountryOfOwner = businessDataItem.CountryOfOwner,
                        LegalFormOfOwner = businessDataItem.LegalFormOfOwner,
                        OwnerType = businessDataItem.OwnerType,
                        FromTime = i == startYear
                            ? businessDataItem.FromTime
                            : DateTime.ParseExact($"01.01.{i}", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                        ToTime = i == endYear
                            ? businessDataItem.ToTime
                            : DateTime.ParseExact($"31.12.{i}", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                        IsValid = businessDataItem.IsValid,
                    }
                );
            }

            return resultList;
        }
    }
}
