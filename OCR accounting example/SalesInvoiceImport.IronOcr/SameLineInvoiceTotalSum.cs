using IronOcr;
using System.Collections.Generic;
using System.Linq;

namespace SalesInvoiceImport.IronOcr
{
    public class SameLineInvoiceTotalSum : IInvoiceTotalSum
    {
        public decimal? Find(IReadOnlyList<OcrResult.Line> lines)
        {
            var totalSumLines =
                lines
                .Where(SalesInvoiceTotalSum.IsInvoiceTotalLine)
                .Where(SalesInvoiceTotalSum.EndsWithCurrency)
                .Select(line => line.Text)
                .Select(SalesInvoiceTotalSum.TrimInvoiceTotalSumStart)
                .Select(SalesInvoiceTotalSum.TrimInvoiceTotalSumEnd)
                .Where(SalesInvoiceTotalSum.IsDecimal)
                .Select(SalesInvoiceTotalSum.ToDecimal);

            if (totalSumLines.Any())
            {
                return totalSumLines.First();
            }

            return null;
        }
    }
}
