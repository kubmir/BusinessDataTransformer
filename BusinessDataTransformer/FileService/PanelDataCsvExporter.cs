using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BusinessDataTransformer.Model;

namespace BusinessDataTransformer.FileService
{
    public class PanelDataCsvExporter
    {
        private const int COUNT_OF_TOP_OWNERS = 3;
        private const int START_YEAR = 2010;
        private const int END_YEAR = 2014;

        public void ExportPanelDataToCsv(List<CompanyOutputData> companiesData)
        {
            var fileHeader = GenerateCsvHeader();

            using (StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/Diplomovka_ESF/panel_data.csv", false, Encoding.UTF8))
            {
                file.WriteLine(fileHeader);

                foreach (var companyData in companiesData)
                {
                    var lines = TransformCompanyDataToCsvString(companyData);

                    foreach (var line in lines)
                    {
                        if (line != null)
                        {
                            file.WriteLine(line);
                        }
                    }
                }
            }
        }

        private string GenerateCsvHeader()
        {
            return "ICO;Rok;ROA;ROE;Zahranicny_vlastnik;Institucionalny_vlastnik;Koncentracia_vlastnictva;Jednoosobova_SRO";
        }

        public string[] TransformCompanyDataToCsvString(CompanyOutputData companyData)
        {
            var companyLines = new string[END_YEAR - START_YEAR + 1];

            var index = 0;
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

                    ownerData.RemoveAll(data => !double.TryParse(data.OwnerShare, out _));

                    if (ownerData.Count != 0 && companyData.FinancialResults != null)
                    {
                        var financialDataOfYear = GetFinancialDataOfYear(currentYear, companyData.FinancialResults);
                        var hasForeignOwner = HasForeignOwner(ownerData);
                        var hasInstituionalOwner = HasInstitutionalOwner(ownerData);
                        var isOnePersonSro = IsOnePersonSro(ownerData);
                        var concentration = CalculateOwnershipConcentration(ownerData);

                        if (concentration != -1 && financialDataOfYear.Item1 != "" && financialDataOfYear.Item2 != "")
                        {
                            var dataString = $"{currentYear};{financialDataOfYear.Item1};{financialDataOfYear.Item2};{hasForeignOwner};{hasInstituionalOwner};{GetStringOfFinancialValue(concentration)};{isOnePersonSro}";

                            companyLines[index] = $"{companyData.ICO};{dataString}";
                            index++;
                        }
                    }
                }
            }

            return companyLines;
        }

        private bool HasForeignOwner(List<OwnerInfo> owners)
        {
            var hasForeignOwner = false;

            owners.ForEach(owner =>
            {
                var ownerShare = 0.00;
                double.TryParse(owner.OwnerShare, out ownerShare);

                if (owner.OwnerCountrySign == "ZAHR" && ownerShare > 10)
                {
                    hasForeignOwner = true;
                }
            });

            return hasForeignOwner;
        }

        private bool HasInstitutionalOwner(List<OwnerInfo> owners)
        {
            return owners.Any(owner => owner.OwnerType == "PO");
        }

        private bool IsOnePersonSro(List<OwnerInfo> owners)
        {
            if (owners.Count == 1) {
                var owner = owners.First();
                double ownerShare;
                double.TryParse(owner.OwnerShare, out ownerShare);

                double fullOwnership;
                double.TryParse("100", out fullOwnership);

                return owner.OwnerType == "FO" && ownerShare == fullOwnership;
            }

            return false;
        }


        private double CalculateOwnershipConcentration(List<OwnerInfo> owners)
        {
            var concentration = 0.00;

            for (int i = 0; i < owners.Count; i++)
            {
                var owner = owners.ElementAt(i);
                double ownerShare;

                if (double.TryParse(owner.OwnerShare, out ownerShare))
                {
                    concentration += Math.Pow(ownerShare, 2);
                } else
                {
                    return -1;
                }
            }

            return concentration;
        }

        private Tuple<string, string> GetFinancialDataOfYear(int year, FinancialResultsDataItem financialResultsData)
        {
            switch (year)
            {
                case 2010:
                    return new Tuple<string, string>(GetStringOfFinancialValue(financialResultsData.Roa2010), GetStringOfFinancialValue(financialResultsData.Roe2010));
                case 2011:
                    return new Tuple<string, string>(GetStringOfFinancialValue(financialResultsData.Roa2011), GetStringOfFinancialValue(financialResultsData.Roe2011));
                case 2012:
                    return new Tuple<string, string>(GetStringOfFinancialValue(financialResultsData.Roa2012), GetStringOfFinancialValue(financialResultsData.Roe2012));
                case 2013:
                    return new Tuple<string, string>(GetStringOfFinancialValue(financialResultsData.Roa2013), GetStringOfFinancialValue(financialResultsData.Roe2013));
                case 2014:
                    return new Tuple<string, string>(GetStringOfFinancialValue(financialResultsData.Roa2014), GetStringOfFinancialValue(financialResultsData.Roe2014));
                default:
                    return null;
            }
        }

        private string GetStringOfFinancialValue(double value)
        {
            if (value == 0 || value == double.MaxValue)
            {
                return "";
            }
            else
            {
                return value.ToString("F4");
            }
        }
    }
}
