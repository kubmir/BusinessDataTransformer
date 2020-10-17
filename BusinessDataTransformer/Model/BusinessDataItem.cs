using System;
using System.Globalization;

namespace BusinessDataTransformer.Model
{
    public class BusinessDataItem
    {
        public string OwnerId { get; set; }
        public string ICO { get; set; }
        public string Name { get; set; }
        public string LegalFormOfOwner { get; set; }
        public string CountryOfOwner { get; set; }
        public string OwnerCountrySign { get; set; }
        public string OwnerType { get; set; }
        public string OwnerShare { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string IsValid { get; set; }

        public static BusinessDataItem FromCsv(string businessDataLine)
        {
            string[] values = businessDataLine.Split(';');

            return new BusinessDataItem
            {
                OwnerId = Guid.NewGuid().ToString(),
                ICO = values[0],
                Name = values[1].Replace('"', ' ').Trim(),
                LegalFormOfOwner = values[2],
                CountryOfOwner = values[3],
                OwnerCountrySign = values[4],
                OwnerType = values[5],
                OwnerShare = values[6],
                FromTime = ParseDateFormat(values[7]),
                ToTime = ParseDateFormat(values[8], true),
                IsValid = values[9],
            };
        }

        private static DateTime ParseDateFormat(string date, bool withDefault = false)
        {
            if (!string.IsNullOrEmpty(date))
            {
                var dateParts = date.Split('.');

                if (dateParts.Length == 3)
                {
                    string day = PrepareValidDatePart(dateParts[0]);
                    string month = PrepareValidDatePart(dateParts[1]);
                    string year = dateParts[2].StartsWith('9') ? $"19{dateParts[2]}" : $"20{dateParts[2]}";

                    return DateTime.ParseExact($"{day}.{month}.{year}", "dd.MM.yyyy", CultureInfo.CreateSpecificCulture("cs-CZ"));
                }

                return DateTime.MinValue;
            }

            return DateTime.ParseExact("31.12.2021", "dd.MM.yyyy", CultureInfo.CurrentCulture);
        }

        private static string PrepareValidDatePart(string datePart)
            => datePart.Length == 1 ? $"0{datePart}" : datePart;

        public override string ToString()
            => $"Company {Name} with owner {OwnerId} valid from {FromTime:dd.MM.yyyy} to {ToTime:dd.MM.yyyy}";
    }
}
