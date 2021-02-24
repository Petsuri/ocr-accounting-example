using Accounting;

namespace Builders
{
    public class SalesInvoicePdfServiceBuilder
    {
        private ISalesInvoiceReader reader = new SalesInvoiceReaderStubBuilder().Build();
        private IAccountingEntryFormat format = new AccountingEntryFormatStubBuilder().Build();

        public SalesInvoicePdfServiceBuilder WithReader(ISalesInvoiceReader value)
        {
            reader = value;
            return this;
        }

        public SalesInvoicePdfServiceBuilder WithFormat(IAccountingEntryFormat value)
        {
            format = value;
            return this;
        }

        public SalesInvoicePdfService Build()
        {
            return new(reader, format);
        }
    }
}
