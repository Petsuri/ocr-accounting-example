using SalesInvoiceImport.IronOcr;

namespace Builders
{
    public class IronOcrSalesInvoiceReaderBuilder
    {
        public IronOcrSalesInvoiceReader Build()
        {
            return new IronOcrSalesInvoiceReader(
                new IronOcr.IronTesseract(), new SalesInvoiceTotalSumBuilder().Build()
            );
        }
    }
}
