using IronOcr;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SalesInvoiceImport.IronOcr
{
    public class SameHeightInvoiceTotalSum : IInvoiceTotalSum
    {
        private const int HeightVariationThreshold = 5;

        public decimal? Find(IReadOnlyList<OcrResult.Line> lines)
        {
            var location = GetInvoiceTotalLineLocation(lines);
            if (!location.HasValue)
            {
                return null;
            }

            var minY = location.Value.Y - HeightVariationThreshold;
            var maxY = location.Value.Y + HeightVariationThreshold;
            bool IsWithinAllowedRange(Rectangle r) => minY <= r.Y && r.Y <= maxY;

            var total = lines
                .Where(line => IsWithinAllowedRange(line.Location))
                .Select(line => line.Text)
                .Select(SalesInvoiceTotalSum.TrimInvoiceTotalSumEnd)
                .Where(SalesInvoiceTotalSum.IsDecimal)
                .Select(SalesInvoiceTotalSum.ToDecimal);
            if (total.Any())
            {
                return total.First();
            }

            return null;
        }

        private static Rectangle? GetInvoiceTotalLineLocation(IReadOnlyList<OcrResult.Line> lines)
        {
            var invoiceTotalLineLocations =
                lines
                .Where(SalesInvoiceTotalSum.IsInvoiceTotalLine)
                .Select(line => line.Location);

            if (invoiceTotalLineLocations.Any())
            {
                return invoiceTotalLineLocations.First();
            }

            return null;
        }

    }
}
