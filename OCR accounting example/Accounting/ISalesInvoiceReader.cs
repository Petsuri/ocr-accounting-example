using System.Threading.Tasks;

namespace Accounting
{
    public interface ISalesInvoiceReader
    {
       Task<Result<SalesInvoice>> ReadPdf(byte[] pdf);
    }
}
