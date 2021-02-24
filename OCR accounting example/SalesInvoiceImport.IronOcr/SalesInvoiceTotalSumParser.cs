using IronOcr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace SalesInvoiceImport.IronOcr
{
    internal class SalesInvoiceTotalSumParser
    {
        private const string TotalLineStart = "Invoice Total:";
        private const string TotalSumEndingCurrency = " €";
        private const int HeightVariationThreshold = 5;

        private readonly IEnumerable<OcrResult.Line> lines;

        internal SalesInvoiceTotalSumParser(IEnumerable<OcrResult.Line> lines)
        {
            this.lines = lines;
        }

        public decimal? FindTotalSum()
        {
            var totalSumInLine = GetTotalSumIncludedInSameLine();
            if (totalSumInLine.HasValue)
            {
                return totalSumInLine.Value;
            }

            return FindFromInvoiceTotalHeight();
        }

        private decimal? FindFromInvoiceTotalHeight()
        {
            var location = GetInvoiceTotalLineLocation();
            if (!location.HasValue)
            {
                return null;
            }

            var minY = location.Value.Y - HeightVariationThreshold;
            var maxY = location.Value.Y + HeightVariationThreshold;
            Func<Rectangle, bool> IsWithinAllowedRange = (Rectangle r) => minY <= r.Y && r.Y <= maxY;

            var total = lines
                .Where(line => IsWithinAllowedRange(line.Location))
                .Select(line => line.Text)
                .Select(text=> text.TrimEnd(TotalSumEndingCurrency.ToCharArray()))
                .Where(lineText => IsDecimal(lineText))
                .Select(lineText => ToDecimal(lineText));
            if (total.Any())
            {
                return total.First();
            }

            return null;
        }

        private Rectangle? GetInvoiceTotalLineLocation()
        {
            var invoiceTotalLineLocations =
                lines
                .Where(IsInvoiceTotalLine)
                .Select(line => line.Location);

            if (invoiceTotalLineLocations.Any())
            {
                return invoiceTotalLineLocations.First();
            }

            return null;
        }

        private decimal? GetTotalSumIncludedInSameLine()
        {
            var totalSumLines =
                lines
                .Where(IsInvoiceTotalLine)
                .Where(line => line.Text.EndsWith(TotalSumEndingCurrency))
                .Select(line => line.Text)
                .Select(text => text.TrimStart(TotalLineStart.ToCharArray()))
                .Select(text => text.TrimEnd(TotalSumEndingCurrency.ToCharArray()))
                .Where(IsDecimal)
                .Select(ToDecimal);

            if (totalSumLines.Any())
            {
                return totalSumLines.First();
            }

            return null;
        }

        private static bool IsInvoiceTotalLine(OcrResult.Line line)
        {
            return line.Text.StartsWith(TotalLineStart);
        }

        private static bool IsDecimal(string value)
        {
            return decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var _);
        }

        private static decimal ToDecimal(string value)
        {
            return decimal.Parse(value, NumberStyles.Currency, CultureInfo.InvariantCulture);
        }
    }
}
