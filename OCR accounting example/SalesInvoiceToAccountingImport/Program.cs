using SalesInvoiceImport.IronOcr;
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
            var test = File.ReadAllBytes("./TestData/example-sales-invoice1.pdf");
            var invoiceReader = new IronOcrSalesInvoiceReader(
                new IronOcr.IronTesseract(), 
                new SalesInvoiceTotalSum(new IInvoiceTotalSum[] { new SameLineInvoiceTotalSum(), new SameHeightInvoiceTotalSum() });
            await invoiceReader.ReadPdf(test);
            Console.WriteLine("Hello World!");
        }
    }
}
