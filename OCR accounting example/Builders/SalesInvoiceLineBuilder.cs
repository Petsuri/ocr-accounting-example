using Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builders
{
    public class SalesInvoiceLineBuilder
    {
        private ProductName name = new("");
        private int amount = 1;
        private ProductUnit unit = new("pcs");
        private decimal unitNetPrice = 1m;
        private FinnishVatPercentage vatPercentage = new(0);

        public SalesInvoiceLineBuilder WithName(string value)
        {
            name = new(value);
            return this;
        }

        public SalesInvoiceLineBuilder WithAmount(int value)
        {
            amount = value;
            return this;
        }

        public SalesInvoiceLineBuilder WithUnit(string value)
        {
            unit = new(value);
            return this;
        }

        public SalesInvoiceLineBuilder WithUnitNetPrice(decimal value)
        {
            unitNetPrice = value;
            return this;
        }

        public SalesInvoiceLineBuilder WithVatPercentage(int value)
        {
            vatPercentage = new(value);
            return this;
        }

        public SalesInvoiceLine Build()
        {
            return new SalesInvoiceLine(name, amount, unit, unitNetPrice, vatPercentage);
        }

        public static implicit operator SalesInvoiceLine(SalesInvoiceLineBuilder value)
        {
            return value.Build();
        }
    }
}
