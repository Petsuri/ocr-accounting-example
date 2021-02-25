using Accounting;
using Serilog.Core;
using System;
using System.Threading.Tasks;

namespace SalesInvoiceToAccountingImport
{
    class SalesInvoiceToAccountingImportCommand
    {
        private readonly SalesInvoicePdfService pdfService;
        private readonly Logger logger;

        public SalesInvoiceToAccountingImportCommand(SalesInvoicePdfService pdfService, Logger logger)
        {
            this.pdfService = pdfService;
            this.logger = logger;
        }

        public async Task SalesInvoicePdfToCsv(string pdfName, byte[] pdf)
        {
            var result = await pdfService.ToImportFormat(pdf);
            if (result.IsOk)
            {
                logger.Information("CSV for {pdfName}: " + Environment.NewLine + "{csv}", pdfName, result.Value);
            } else
            {
                logger.Error("Unable to convert {pdfName} to accounting entry: {error}", pdfName, result.Error);
            }
        }
    }
}
