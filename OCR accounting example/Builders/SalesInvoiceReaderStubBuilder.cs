using Accounting;
using NSubstitute;

namespace Builders
{
    public class SalesInvoiceReaderStubBuilder
    {
        private Result<SalesInvoice> readPdf = Result<SalesInvoice>.Ok(new SalesInvoiceBuilder());

        public SalesInvoiceReaderStubBuilder WithReadPdfOk(SalesInvoice value)
        {
            readPdf = Result<SalesInvoice>.Ok(value);
            return this;
        }

        public SalesInvoiceReaderStubBuilder WithReadPdfFailure(string value)
        {
            readPdf = Result<SalesInvoice>.Failure(value);
            return this;
        }

        public ISalesInvoiceReader Build()
        {
            var stub = Substitute.For<ISalesInvoiceReader>();
            stub.ReadPdf(Arg.Any<byte[]>()).Returns(readPdf);

            return stub;
        }
    }
}
