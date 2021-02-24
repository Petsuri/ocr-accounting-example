using Accounting;
using System.Collections.Generic;

namespace Builders
{
    public class SalesInvoiceBuilder
    {
        private decimal invoiceTotal = 0m;
        private List<SalesInvoiceLine> lines = new List<SalesInvoiceLine>();

        public SalesInvoiceBuilder WithInvoiceTotal(decimal value)
        {
            invoiceTotal = value;
            return this;
        }

        public SalesInvoiceBuilder WithLine(SalesInvoiceLine line)
        {
            lines.Add(line);
            return this;
        }

        public SalesInvoice Build()
        {
            return new(invoiceTotal, lines);
        }

        public static implicit operator SalesInvoice(SalesInvoiceBuilder value)
        {
            return value.Build();
        }
    }
}
