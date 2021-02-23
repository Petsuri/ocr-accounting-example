using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting
{
    public interface ISalesInvoiceReader
    {
       Task<IReadOnlyList<SalesInvoiceLine>> ReadLines(byte[] pdf);
    }
}
