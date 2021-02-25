using IronOcr;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SalesInvoiceImport.IronOcr
{
    public class SalesInvoiceTotalSum: IInvoiceTotalSum
    {
        internal const string TotalLineStart = "Invoice Total:";
        internal const string TotalSumEndingCurrency = " €";

        private readonly IReadOnlyList<IInvoiceTotalSum> totalSums;

        public SalesInvoiceTotalSum(IEnumerable<IInvoiceTotalSum> totalSums)
        {
            this.totalSums = totalSums.ToList();
        }


        public decimal? Find(IReadOnlyList<OcrResult.Line> lines)
        {
            foreach(var sum in totalSums)
            {
                var invoiceTotal = sum.Find(lines);
                if (invoiceTotal.HasValue)
                {
                    return invoiceTotal;
                }
            }

            return null;
        }

        internal static bool IsInvoiceTotalLine(OcrResult.Line line)
        {
            return line.Text.StartsWith(TotalLineStart);
        }

        internal static bool EndsWithCurrency(OcrResult.Line line)
        {
            return line.Text.EndsWith(TotalSumEndingCurrency);
        }

        internal static string TrimInvoiceTotalSumStart(string text)
        {
            return text.TrimStart(TotalLineStart.ToCharArray());
        }

        internal static string TrimInvoiceTotalSumEnd(string text)
        {
            return text.TrimEnd(TotalSumEndingCurrency.ToCharArray());
        }

        internal static bool IsDecimal(string value)
        {
            return decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal _);
        }

        internal static decimal ToDecimal(string value)
        {
            return decimal.Parse(value, NumberStyles.Currency, CultureInfo.InvariantCulture);
        }
    }
}
