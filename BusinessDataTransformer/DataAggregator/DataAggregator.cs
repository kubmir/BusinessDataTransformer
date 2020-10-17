using System;
using System.Collections.Generic;
using System.Linq;
using BusinessDataTransformer.Model;

namespace BusinessDataTransformer.Aggregators
{
    public class DataAggregator
    {
        public List<CompanyOutputData> AggregateDataByCompany(List<BusinessDataItem> businessDataItems)
        {
            return businessDataItems
                    .GroupBy(businessDataItem => businessDataItem.ICO)
                    .Select(companyData =>
                    {
                        Dictionary<int, List<OwnerInfo>> ownersByYears = new Dictionary<int, List<OwnerInfo>>();

                        foreach (var companyByYearAndOwner in companyData)
                        {
                            var ownerInfo = new OwnerInfo
                            {
                                OwnerId = companyByYearAndOwner.OwnerId,
                                CountryOfOwner = companyByYearAndOwner.CountryOfOwner,
                                OwnerCountrySign = companyByYearAndOwner.OwnerCountrySign,
                                OwnerShare = companyByYearAndOwner.OwnerShare,
                                OwnerType = companyByYearAndOwner.OwnerType,
                                LegalFormOfOwner = companyByYearAndOwner.LegalFormOfOwner,
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

                        return new CompanyOutputData
                        {
                            ICO = companyData.First().ICO,
                            Name = companyData.First().Name,
                            OwnersByYears = ownersByYears,
                        };
                    })
                    .ToList();

        }
    }
}
