using System;
using OfficeOpenXml;

namespace BusinessDataTransformer.Model
{
    public class FinancialResultsDataItem
    {
        public string ICO { get; set; }
        public string Section { get; set; }

        public double Assets2010 { get; set; }
        public double Assets2011 { get; set; }
        public double Assets2012 { get; set; }
        public double Assets2013 { get; set; }
        public double Assets2014 { get; set; }

        public double Equity2010 { get; set; }
        public double Equity2011 { get; set; }
        public double Equity2012 { get; set; }
        public double Equity2013 { get; set; }
        public double Equity2014 { get; set; }

        public double Ebit2010 { get; set; }
        public double Ebit2011 { get; set; }
        public double Ebit2012 { get; set; }
        public double Ebit2013 { get; set; }
        public double Ebit2014 { get; set; }

        public double Roa2010 { get; set; }
        public double Roa2011 { get; set; }
        public double Roa2012 { get; set; }
        public double Roa2013 { get; set; }
        public double Roa2014 { get; set; }

        public double Roe2010 { get; set; }
        public double Roe2011 { get; set; }
        public double Roe2012 { get; set; }
        public double Roe2013 { get; set; }
        public double Roe2014 { get; set; }

        public static FinancialResultsDataItem FromExcel(ExcelWorksheet worksheet, int row)
        {
            var Assets2010 = parseDouble(worksheet.Cells[row, 17].ToText());
            var Assets2011 = parseDouble(worksheet.Cells[row, 18].ToText());
            var Assets2012 = parseDouble(worksheet.Cells[row, 19].ToText());
            var Assets2013 = parseDouble(worksheet.Cells[row, 20].ToText());
            var Assets2014 = parseDouble(worksheet.Cells[row, 21].ToText());

            var Equity2010 = parseDouble(worksheet.Cells[row, 32].ToText());
            var Equity2011 = parseDouble(worksheet.Cells[row, 33].ToText());
            var Equity2012 = parseDouble(worksheet.Cells[row, 34].ToText());
            var Equity2013 = parseDouble(worksheet.Cells[row, 35].ToText());
            var Equity2014 = parseDouble(worksheet.Cells[row, 36].ToText());

            var Ebit2010 = parseDouble(worksheet.Cells[row, 47].ToText());
            var Ebit2011 = parseDouble(worksheet.Cells[row, 48].ToText());
            var Ebit2012 = parseDouble(worksheet.Cells[row, 49].ToText());
            var Ebit2013 = parseDouble(worksheet.Cells[row, 50].ToText());
            var Ebit2014 = parseDouble(worksheet.Cells[row, 51].ToText());


            return new FinancialResultsDataItem
            {
                ICO = worksheet.Cells[row, 1].ToText(),
                Section = parseSection(worksheet.Cells[row, 5].ToText()),

                Assets2010 = Assets2010,
                Assets2011 = Assets2011,
                Assets2012 = Assets2012,
                Assets2013 = Assets2013,
                Assets2014 = Assets2014,

                Equity2010 = Equity2010,
                Equity2011 = Equity2011,
                Equity2012 = Equity2012,
                Equity2013 = Equity2013,
                Equity2014 = Equity2014,

                Ebit2010 = Ebit2010,
                Ebit2011 = Ebit2011,
                Ebit2012 = Ebit2012,
                Ebit2013 = Ebit2013,
                Ebit2014 = Ebit2014,

                Roa2010 = Assets2010 <= 0.00 ? double.MaxValue : Ebit2010 / Assets2010,
                Roa2011 = Assets2011 <= 0.00 ? double.MaxValue : Ebit2011 / Assets2011,
                Roa2012 = Assets2012 <= 0.00 ? double.MaxValue : Ebit2012 / Assets2012,
                Roa2013 = Assets2013 <= 0.00 ? double.MaxValue : Ebit2013 / Assets2013,
                Roa2014 = Assets2014 <= 0.00 ? double.MaxValue : Ebit2014 / Assets2014,

                Roe2010 = Equity2010 == 0.00 || (Equity2010 < 0 && Ebit2010 < 0)
                    ? double.MaxValue
                    : Ebit2010 / Equity2010,
                Roe2011 = Equity2011 == 0.00 || (Equity2011 < 0 && Ebit2011 < 0)
                    ? double.MaxValue
                    : Ebit2011 / Equity2011,
                Roe2012 = Equity2012 == 0.00 || (Equity2012 < 0 && Ebit2012 < 0)
                    ? double.MaxValue
                    : Ebit2012 / Equity2012,
                Roe2013 = Equity2013 == 0.00 || (Equity2013 < 0 && Ebit2013 < 0)
                    ? double.MaxValue
                    : Ebit2013 / Equity2013,
                Roe2014 = Equity2014 == 0.00 || (Equity2014 < 0 && Ebit2014 < 0)
                    ? double.MaxValue
                    : Ebit2014 / Equity2014,
            };
        }

        private static double parseDouble(string value)
        {
            var parsedNumber = 0.00;
            double.TryParse(value, out parsedNumber);
        
            return parsedNumber;
        }

        private static string parseSection(string section)
        {
            return section[0].ToString();
        }

        public override string ToString() => $"{ICO} - {Roa2010} - {Roa2011} - {Roa2012} - {Roa2013} - {Roa2014}";
    }
}
