using Accounting;
using IronOcr;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SalesInvoiceImport.IronOcr
{
    internal class SalesInvoiceLinesParser
    {
        private const int VatIndex = 0;
        private const int ExcludingVatPriceIndex = 1;
        private const int UnitPriceIndex = 2;
        private const int UnitIndex = 3;
        private const int AmountIndex = 4;

        private readonly IEnumerable<OcrResult.Line> lines;

        internal SalesInvoiceLinesParser(IEnumerable<OcrResult.Line> lines)
        {
            this.lines = lines;
        }

        public IReadOnlyList<SalesInvoiceLine> FindLines()
        {
            return 
                lines
                .Where(IsProductLine)
                .Select(ToProductLine)
                .ToList();
        }

        private static bool IsProductLine(OcrResult.Line line)
        {
            var reversedOrder = line.Words.Reverse().ToList();
            return ContainsAmount(reversedOrder) &&
                ContainsUnit(reversedOrder) &&
                ContainsFinnishVatPercentage(reversedOrder) &&
                ContainsPriceInformation(reversedOrder);
        }

        private static bool ContainsAmount(IReadOnlyList<OcrResult.Word> words)
        {
            if (words.Count <= AmountIndex)
            {
                return false;
            }

            return int.TryParse(words[AmountIndex].Text, out var _);
        }

        private static bool ContainsUnit(IReadOnlyList<OcrResult.Word> words)
        {
            if (words.Count <= UnitIndex)
            {
                return false;
            }

            return ProductUnit.IsValid(words[UnitIndex].Text);
        }

        private static bool ContainsFinnishVatPercentage(IReadOnlyList<OcrResult.Word> words)
        {
            if (!words.Any())
            {
                return false;
            }


            if (int.TryParse(words[VatIndex].Text, out var integerValue))
            {
                return FinnishVatPercentage.IsValid(integerValue);
            }

            return false;
        }

        private static bool ContainsPriceInformation(IReadOnlyList<OcrResult.Word> words)
        {
            if (words.Count <= UnitPriceIndex)
            {
                return false;
            }

            return IsDecimal(words[ExcludingVatPriceIndex]) &&
                IsDecimal(words[UnitPriceIndex]);
        }

        private static SalesInvoiceLine ToProductLine(OcrResult.Line line)
        {
            const int ToProductName = 5;

            var reversedOrder = line.Words.Reverse().ToList();
            var productWords = reversedOrder.Skip(ToProductName).Reverse();
            return new SalesInvoiceLine(
                new(string.Join(" ", productWords)),
                int.Parse(reversedOrder[AmountIndex].Text),
                new(reversedOrder[UnitIndex].Text),
                ToDecimal(reversedOrder[ExcludingVatPriceIndex]),
                new(int.Parse(reversedOrder[VatIndex].Text))
            );
        }

        private static bool IsDecimal(OcrResult.Word value)
        {
            return decimal.TryParse(value.Text, NumberStyles.Currency, CultureInfo.InvariantCulture, out var _);
        }

        private static decimal ToDecimal(OcrResult.Word value)
        {
            return decimal.Parse(value.Text, NumberStyles.Currency, CultureInfo.InvariantCulture);
        }

    }
}
