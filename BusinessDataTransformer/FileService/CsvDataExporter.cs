using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BusinessDataTransformer.Model;

namespace BusinessDataTransformer.FileService
{
    public class CsvDataExporter
    {
        private const int COUNT_OF_TOP_OWNERS = 3;
        private const int START_YEAR = 2012;
        private const int END_YEAR = 2014;

        public void ExportDataToCsv(List<CompanyOutputData> companiesData)
        {
            var fileHeader = GenerateCsvHeader();

            using (StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/Diplomovka_ESF/final_v2.csv", false, Encoding.UTF8))
            {
                file.WriteLine(fileHeader);

                foreach (var companyData in companiesData)
                {
                    var line = TransformCompanyDataToCsvString(companyData);

                    if (line != null)
                    {
                        file.WriteLine(line);
                    }
                }
            }
        }

        private string GenerateCsvHeader()
        {
            var basicCompanyInfoHeader = $"ICO;Nazov";

            var companyDataByYearsHeader = "";


            for (int currentYear = START_YEAR; currentYear <= END_YEAR; currentYear++)
            {
                for (int i = 0; i < COUNT_OF_TOP_OWNERS; i++)
                {
                    var countOfOwner = i + 1;
                    companyDataByYearsHeader += $"{currentYear}-{countOfOwner}.Majitel-PravnaForma;" +
                        $"{currentYear}-{countOfOwner}.Majitel-Krajina;{currentYear}-{countOfOwner}.Majitel-KrajinaPriznak;" +
                        $"{currentYear}-{countOfOwner}.Majitel-Podiel;{currentYear}-{countOfOwner}.Majitel-Typ;";
                }

                companyDataByYearsHeader += $"Assets{currentYear};Equity{currentYear};Ebit{currentYear};ROA{currentYear};ROE{currentYear};";
            }

            return $"{basicCompanyInfoHeader};{companyDataByYearsHeader}";
        }

        public string TransformCompanyDataToCsvString(CompanyOutputData companyData)
        {
            var basicCompanyInfo = $"{companyData.ICO};{companyData.Name}";

            var companyDataByYears = "";

            if (companyData.FinancialResults == null ||
                (companyData.FinancialResults?.Roa2012 == 0
                    && companyData.FinancialResults?.Roa2013 == 0
                    && companyData.FinancialResults?.Roa2014 == 0
                    && companyData.FinancialResults?.Roe2012 == 0
                    && companyData.FinancialResults?.Roe2013 == 0
                    && companyData.FinancialResults?.Roe2014 == 0)
            )
            {
                return null;
            }

            for (int currentYear = START_YEAR; currentYear <= END_YEAR; currentYear++)
            {
                if (companyData.OwnersByYears.ContainsKey(currentYear))
                {
                    companyData.OwnersByYears[currentYear].Sort(delegate (OwnerInfo x, OwnerInfo y)
                    {
                        if (x.OwnerShare == null && y.OwnerShare == null) return 0;
                        else if (x.OwnerShare == null) return -1;
                        else if (y.OwnerShare == null) return 1;
                        else return y.OwnerShare.CompareTo(x.OwnerShare);
                    });

                    var ownersCount = companyData.OwnersByYears[currentYear].Count;
                    var ownerData = ownersCount > COUNT_OF_TOP_OWNERS
                        ? companyData.OwnersByYears[currentYear].GetRange(0, COUNT_OF_TOP_OWNERS)
                        : companyData.OwnersByYears[currentYear];

                    foreach (var owner in ownerData)
                    {
                        // Remove companies (from result csv) with not numeric owner share
                        if (!double.TryParse(owner.OwnerShare, out _))
                        {
                            return null;
                        }

                        companyDataByYears += $"{owner.LegalFormOfOwner};{owner.CountryOfOwner};{owner.OwnerCountrySign};{owner.OwnerShare};{owner.OwnerType};";
                    }

                    if (ownersCount < COUNT_OF_TOP_OWNERS)
                    {
                        var missingOwnersCount = COUNT_OF_TOP_OWNERS - ownersCount;

                        for (var i = 0; i < missingOwnersCount; i++)
                        {
                            companyDataByYears += ";;;;;";
                        }
                    }

                    if (currentYear == 2012)
                    {
                        companyDataByYears += $"{companyData.FinancialResults?.Assets2012};{companyData.FinancialResults?.Equity2012};{companyData.FinancialResults?.Ebit2012};{GetStringOfFinancialValue(companyData.FinancialResults.Roa2012)};{GetStringOfFinancialValue(companyData.FinancialResults.Roe2012)};";
                    }

                    if (currentYear == 2013)
                    {
                        companyDataByYears += $"{companyData.FinancialResults?.Assets2013};{companyData.FinancialResults?.Equity2013};{companyData.FinancialResults?.Ebit2013};{GetStringOfFinancialValue(companyData.FinancialResults.Roa2013)};{GetStringOfFinancialValue(companyData.FinancialResults.Roe2013)};";
                    }

                    if (currentYear == 2014)
                    {
                        companyDataByYears += $"{companyData.FinancialResults?.Assets2014};{companyData.FinancialResults?.Equity2014};{companyData.FinancialResults?.Ebit2014};{GetStringOfFinancialValue(companyData.FinancialResults.Roa2014)};{GetStringOfFinancialValue(companyData.FinancialResults.Roe2014)};";
                    }
                }
            }

            if (companyDataByYears == "")
            {
                return null;
            }

            return $"{basicCompanyInfo};{companyDataByYears}";
        }

        private string GetStringOfFinancialValue(double value)
        {
            if (value == 0 || value == double.MaxValue)
            {
                return "";
            } else
            {
                return value.ToString("F4");
            }
        }
    }
}
