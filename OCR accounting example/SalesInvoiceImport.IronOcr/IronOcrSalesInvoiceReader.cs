using Accounting;
using IronOcr;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesInvoiceImport.IronOcr
{
    public class IronOcrSalesInvoiceReader : ISalesInvoiceReader
    {
        private static readonly IReadOnlyList<string> ProductUnits = new List<string>()
        {
            "pcs"
        };

        private const int VatIndex = 0;
        private const int ExcludingVatPriceIndex = 1;
        private const int UnitPriceIndex = 2;
        private const int UnitIndex = 3;
        private const int AmountIndex = 4;
        

        private readonly IronTesseract ocr;

        public IronOcrSalesInvoiceReader(IronTesseract ocr)
        {
            this.ocr = ocr;
        }

        public async Task<IReadOnlyList<SalesInvoiceLine>> ReadLines(byte[] pdf)
        {
            var result = await ReadPdf(pdf);
            var productLines = result.Lines
                .Where(IsProductLine)
                .Select(ToProductLine)
                .ToList();

            return new List<SalesInvoiceLine>();
        }

        private async Task<OcrResult> ReadPdf(byte[] pdf)
        {
            using (var input = new OcrInput())
            {
                input.AddPdf(pdf);
                input.DeNoise();
                return await ocr.ReadAsync(input);
            }
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

            return ProductUnits.Contains(words[UnitIndex].Text);
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

            if (!decimal.TryParse(words[ExcludingVatPriceIndex].Text, out var _))
            {
                return false;
            }

            if (!decimal.TryParse(words[UnitPriceIndex].Text, out var _))
            {
                return false;
            }

            return true;
        }

        private static SalesInvoiceLine ToProductLine(OcrResult.Line line)
        {
            const int ToProductName = 5;

            var reversedOrder = line.Words.Reverse().ToList();
            var productWords = reversedOrder.Skip(ToProductName).Reverse();
            return new SalesInvoiceLine(
                string.Join(" ", productWords),
                int.Parse(reversedOrder[AmountIndex].Text),
                reversedOrder[UnitIndex].Text,
                decimal.Parse(reversedOrder[UnitPriceIndex].Text),
                new FinnishVatPercentage(int.Parse(reversedOrder[VatIndex].Text))
            );
        }
    }
}
