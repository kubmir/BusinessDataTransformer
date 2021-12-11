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

            using (StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + $"/Diplomovka_ESF/panel_data.csv", false, Encoding.UTF8))
            {
                file.WriteLine(fileHeader);

                foreach (var companyData in companiesData)
                {
                    var lines = TransformCompanyDataToCsvString(companyData);

                    if (lines == null)
                    {
                        continue;
                    }

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
            return $"ICO;Rok;ROA;ROE;Zahranicny_vlastnik;Institucionalny_vlastnik;Statny_vlastnik;Koncentracia_vlastnictva_h3;Koncentracia_vlastnictva_h5;Koncentracia_vlastnictva_t3;Koncentracia_vlastnictva_t5;Jednoosobova_SRO;Sekcia";
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
                        return y.OwnerShare.CompareTo(x.OwnerShare);
                    });

                    var ownersCount = companyData.OwnersByYears[currentYear].Count;
                    var ownerData = ownersCount > COUNT_OF_TOP_OWNERS
                        ? companyData.OwnersByYears[currentYear].GetRange(0, COUNT_OF_TOP_OWNERS)
                        : companyData.OwnersByYears[currentYear];

                    if (ownerData.Count != 0 && companyData.FinancialResults != null)
                    {
                        var financialDataOfYear = GetFinancialDataOfYear(currentYear, companyData.FinancialResults);
                        var hasForeignOwner = HasForeignOwner(ownerData);
                        var hasInstituionalOwner = HasInstitutionalOwner(ownerData);
                        var isOnePersonSro = IsOnePersonSro(ownerData);
                        var hasStateOwner = HasStateOwner(ownerData);
                        var (h3, t3) = CalculateOwnershipConcentration(companyData.OwnersByYears[currentYear], 3);
                        var (h5, t5) = CalculateOwnershipConcentration(companyData.OwnersByYears[currentYear], 5);

                        if (financialDataOfYear.Item1 != "" && financialDataOfYear.Item2 != "")
                        {
                            var dataString = $"{currentYear};{financialDataOfYear.Item1};{financialDataOfYear.Item2};{hasForeignOwner};{hasInstituionalOwner};{hasStateOwner};{GetStringOfFinancialValue(h3 / 100)};{GetStringOfFinancialValue(h5 / 100)};{GetStringOfFinancialValue(t3)};{GetStringOfFinancialValue(t5)};{isOnePersonSro};{companyData.FinancialResults.Section}";

                            companyLines[index] = $"{companyData.ICO};{dataString}";
                            index++;
                        }
                    }
                }
            }

            return companyLines.Any(line => line == null) ? null : companyLines;
        }

        private bool HasForeignOwner(List<OwnerInfo> owners)
        {
            var hasForeignOwner = false;

            owners.ForEach(owner =>
            {
                if (owner.OwnerCountrySign == "ZAHR" && owner.OwnerShare > 10)
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

        private bool HasStateOwner(List<OwnerInfo> owners)
        {
            return owners.Any(owner =>
            {
                var legalForm = owner.LegalFormOfOwner.ToLower();

                return legalForm.Contains("svazek obcí") ||
                    legalForm.Contains("obec") || legalForm.Contains("kraj") ||
                    legalForm.Contains("organizační složka státu") ||
                    legalForm.Contains("státní podnik") || legalForm.Contains("organizační složka státu") ||
                    legalForm.Contains("fond (národního majetku, pozemkový)");
            });
        }

        private bool IsOnePersonSro(List<OwnerInfo> owners)
        {
            if (owners.Count == 1) {
                var owner = owners.First();

                double fullOwnership;
                double.TryParse("100", out fullOwnership);

                return owner.OwnerType == "FO" && owner.OwnerShare == fullOwnership;
            }

            return false;
        }


        private Tuple<double, double> CalculateOwnershipConcentration(List<OwnerInfo> owners, int maxCount)
        {
            var herfindahlIndex = 0.00;
            var topIndex = 0.00;
            var count = owners.Count > maxCount ? maxCount : owners.Count;

            for (int i = 0; i < count; i++)
            {
                var owner = owners.ElementAt(i);

                herfindahlIndex += Math.Pow(owner.OwnerShare, 2);
                topIndex += owner.OwnerShare;
            }

            return new Tuple<double, double>(herfindahlIndex, topIndex);
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
