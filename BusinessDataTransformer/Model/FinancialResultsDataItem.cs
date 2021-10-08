using System;

namespace BusinessDataTransformer.Model
{
    public class FinancialResultsDataItem
    {
        public string ICO { get; set; }
        public double Assets2013 { get; set; }
        public double Assets2014 { get; set; }
        public double Equity2012 { get; set; }
        public double Assets2012 { get; set; }
        public double Equity2013 { get; set; }
        public double Equity2014 { get; set; }
        public double Ebit2012 { get; set; }
        public double Ebit2013 { get; set; }
        public double Ebit2014 { get; set; }
        public double Roa2012 { get; set; }
        public double Roa2013 { get; set; }
        public double Roa2014 { get; set; }
        public double Roe2012 { get; set; }
        public double Roe2013 { get; set; }
        public double Roe2014 { get; set; }

        public static FinancialResultsDataItem FromExcel(String excelRow)
        {
            string[] values = excelRow.Split(',');
            var Assets2012 = parseDouble(values[18]);
            var Assets2013 = parseDouble(values[19]);
            var Assets2014 = parseDouble(values[20]);
            var Equity2012 = parseDouble(values[33]);
            var Equity2013 = parseDouble(values[34]);
            var Equity2014 = parseDouble(values[35]);
            var Ebit2012 = parseDouble(values[48]);
            var Ebit2013 = parseDouble(values[49]);
            var Ebit2014 = parseDouble(values[50]);


            return new FinancialResultsDataItem
            {
                ICO = values[0],
                Assets2012 = Assets2012,
                Assets2013 = Assets2013,
                Assets2014 = Assets2014,
                Equity2012 = Equity2012,
                Equity2013 = Equity2013,
                Equity2014 = Equity2014,
                Ebit2012 = Ebit2012,
                Ebit2013 = Ebit2013,
                Ebit2014 = Ebit2014,
                Roa2012 = Assets2012 == 0.00 ? double.MaxValue : Ebit2012 / Assets2012,
                Roa2013 = Assets2013 == 0.00 ? double.MaxValue : Ebit2013 / Assets2013,
                Roa2014 = Assets2014 == 0.00 ? double.MaxValue : Ebit2014 / Assets2014,
                Roe2012 = Equity2012 <= 0.00 ? double.MaxValue : Ebit2012 / Equity2012,
                Roe2013 = Equity2013 <= 0.00 ? double.MaxValue : Ebit2013 / Equity2013,
                Roe2014 = Equity2014 <= 0.00 ? double.MaxValue : Ebit2014 / Equity2014,
            };
        }

        private static double parseDouble(string value)
        {
            var parsedNumber = 0.00;
            double.TryParse(value, out parsedNumber);
        
            return parsedNumber;
        }

    }
}
