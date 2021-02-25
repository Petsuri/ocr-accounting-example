using Accounting;
using AccountingEntryImport.CsvHelper;
using SalesInvoiceImport.IronOcr;
using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SalesInvoiceToAccountingImport
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute().Wait();
        }

        private static async Task Execute()
        {

            var empty = File.ReadAllBytes("./TestData/empty.pdf");
            var invoice1 = File.ReadAllBytes("./TestData/example-sales-invoice1.pdf");
            var invoice2 = File.ReadAllBytes("./TestData/example-sales-invoice2.pdf");

            var command = new SalesInvoiceToAccountingImportCommand(CreateSalesInvoiceService(), CreateLogger());
            await command.SalesInvoicePdfToCsv("empty.pdf", empty);
            await command.SalesInvoicePdfToCsv("example-sales-invoice1.pdf", invoice1);
            await command.SalesInvoicePdfToCsv("example-sales-invoice2.pdf", invoice2);
        }



        private static SalesInvoicePdfService CreateSalesInvoiceService()
        {
            return new SalesInvoicePdfService(
                new IronOcrSalesInvoiceReader(
                new IronOcr.IronTesseract(),
                new SalesInvoiceTotalSum(new IInvoiceTotalSum[] { new SameLineInvoiceTotalSum(), new SameHeightInvoiceTotalSum() })),
                new AccountingEntryFormatCsv()
            );
        }

        private static Logger CreateLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}