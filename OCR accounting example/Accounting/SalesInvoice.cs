using System.Collections.Generic;
using System.Linq;

namespace Accounting
{
    public sealed class SalesInvoice
    {
        public decimal InvoiceTotal { get; }
        public IReadOnlyList<SalesInvoiceLine> Lines { get; }

        public SalesInvoice(decimal invoiceTotal, IEnumerable<SalesInvoiceLine> lines)
        {
            InvoiceTotal = invoiceTotal;
            Lines = lines.ToList();
        }

    }
}
