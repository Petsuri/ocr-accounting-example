using Accounting;
using IronOcr;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalesInvoiceImport.IronOcr
{
    public sealed class IronOcrSalesInvoiceReader : ISalesInvoiceReader
    {

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

        private static Result<SalesInvoice> ToSalesInvoice(OcrResult.Line[] lines)
        {
            var totalSumParser = new SalesInvoiceTotalSumParser(lines);
            var linesParser = new SalesInvoiceLinesParser(lines);
            
            var invoiceTotalSum = totalSumParser.FindTotalSum();
            var salesInvoiceLines = linesParser.FindLines();
            if (!salesInvoiceLines.Any() || !invoiceTotalSum.HasValue)
            {
                return Result<SalesInvoice>.Failure("Sales invoice information can't be found from PDF. Please check that this is a valid sales invoice.");

            }

            return Result<SalesInvoice>.Ok(
                new SalesInvoice(invoiceTotalSum.Value, salesInvoiceLines)
            );
        }

     
    }
}
