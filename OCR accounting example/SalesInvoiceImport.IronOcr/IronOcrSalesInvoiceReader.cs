using Accounting;
using IronOcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesInvoiceImport.IronOcr
{
    public sealed class IronOcrSalesInvoiceReader : ISalesInvoiceReader
    {

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

        public async Task<Result<SalesInvoice>> ReadPdf(byte[] pdf)
        {
            if (!pdf.Any())
            {
                return Result<SalesInvoice>.Failure("Empty byte array is not valid PDF file");
            }

            var result = await OcrPdf(pdf);
            if (!result.IsOk)
            {
                return Result<SalesInvoice>.Failure(result.Error);
            }

            return ToSalesInvoice(result.Value.Lines);
        }

        private async Task<Result<OcrResult>> OcrPdf(byte[] pdf)
        {
            try
            {
                using (var input = new OcrInput())
                {
                    input.AddPdf(pdf);
                    input.DeNoise();
                    var ocrResult = await ocr.ReadAsync(input);

                    return Result<OcrResult>.Ok(ocrResult);
                }
            } catch(Exception ex)
            {
                return Result<OcrResult>.Failure(ex.ToString());
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

        private static Result<SalesInvoice> ToSalesInvoice(OcrResult.Line[] lines)
        {
            var productLines = lines
                .Where(IsProductLine)
                .Select(ToProductLine)
                .ToList();
            if (!productLines.Any())
            {
                return Result<SalesInvoice>.Failure("Sales invoice information can't be found from PDF. Please check that this is a valid sales invoice.");

            }

            return Result<SalesInvoice>.Ok(new SalesInvoice());
        }

        private static SalesInvoiceLine ToProductLine(OcrResult.Line line)
        {
            const int ToProductName = 5;

            var reversedOrder = line.Words.Reverse().ToList();
            var productWords = reversedOrder.Skip(ToProductName).Reverse();
            return new SalesInvoiceLine(
                string.Join(" ", productWords),
                int.Parse(reversedOrder[AmountIndex].Text),
                new ProductUnit(reversedOrder[UnitIndex].Text),
                decimal.Parse(reversedOrder[UnitPriceIndex].Text),
                new FinnishVatPercentage(int.Parse(reversedOrder[VatIndex].Text))
            );
        }
    }
}
