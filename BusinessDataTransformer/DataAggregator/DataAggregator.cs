using System;
using System.Collections.Generic;
using System.Linq;
using BusinessDataTransformer.Model;

namespace BusinessDataTransformer.Aggregators
{
    public class DataAggregator
    {
        public List<CompanyOutputData> AggregateDataByCompany(List<BusinessDataItem> businessDataItems, List<FinancialResultsDataItem> financialResultsDataItems)
        {
            return businessDataItems
                    .GroupBy(businessDataItem => businessDataItem.ICO)
                    .Select((companyData, index) =>
                    {
                        if (index % 1000 == 0)
                        {
                            Console.WriteLine("Aggregating company with index " + index);
                        }

                        Dictionary<int, List<OwnerInfo>> ownersByYears = new Dictionary<int, List<OwnerInfo>>();

                        foreach (var companyByYearAndOwner in companyData)
                        {
                            if (companyByYearAndOwner.FromTime.Day == 1 && companyByYearAndOwner.FromTime.Month == 1)
                            {
                                var ownerInfo = new OwnerInfo
                                {
                                    OwnerId = companyByYearAndOwner.OwnerId,
                                    CountryOfOwner = companyByYearAndOwner.CountryOfOwner,
                                    OwnerCountrySign = companyByYearAndOwner.OwnerCountrySign,
                                    OwnerShare = companyByYearAndOwner.OwnerShare,
                                    OwnerType = companyByYearAndOwner.OwnerType,
                                    LegalFormOfOwner = String.IsNullOrEmpty(companyByYearAndOwner.LegalFormOfOwner)
                                        ? companyByYearAndOwner.OwnerType
                                        : companyByYearAndOwner.LegalFormOfOwner,
                                    FromTime = companyByYearAndOwner.FromTime,
                                    ToTime = companyByYearAndOwner.ToTime,
                                };

                                if (ownersByYears.ContainsKey(companyByYearAndOwner.FromTime.Year))
                                {
                                    ownersByYears[companyByYearAndOwner.FromTime.Year].Add(ownerInfo);
                                }
                                else
                                {
                                    ownersByYears.Add(companyByYearAndOwner.FromTime.Year, new List<OwnerInfo> { ownerInfo });
                                }
                            }
                        }

                        var indexOfFinancialResults = financialResultsDataItems.FindIndex(results => results.ICO == companyData.First().ICO);
                        var currentFinancialResults = indexOfFinancialResults == -1 ? null : financialResultsDataItems.ElementAt(indexOfFinancialResults);

                        if (indexOfFinancialResults != -1)
                        {
                            financialResultsDataItems.RemoveAt(indexOfFinancialResults);
                        }

                        return new CompanyOutputData
                        {
                            ICO = companyData.First().ICO,
                            Name = companyData.First().Name,
                            OwnersByYears = ownersByYears,
                            FinancialResults = currentFinancialResults,
                        };
                    })
                    .ToList();

        }
    }
}
