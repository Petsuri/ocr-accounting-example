using Builders;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Tests
{
    class SalesInvoicePdfServiceTests
    {
        [Test]
        public async Task Importing_WithReadingPdfFailing_IsExpected()
        {
            var sut = new SalesInvoicePdfServiceBuilder()
                .WithReader(new SalesInvoiceReaderStubBuilder().WithReadPdfFailure("error").Build())
                .Build();

            var actual = await sut.ToImportFormat(Array.Empty<byte>());

            Assert.IsFalse(actual.IsOk);
            Assert.AreEqual("error", actual.Error);
        }

        [Test]
        public async Task Importing_WithReadingPdfSuccessfully_CreatesCorrectAccountingEntry()
        {
            var invoice = new SalesInvoiceBuilder()
                .WithInvoiceTotal(124m)
                .WithLine(new SalesInvoiceLineBuilder().WithName("petri.works").WithAmount(1).WithUnitNetPrice(100m).WithVatPercentage(24));
            var format = new AccountingEntryFormatStubBuilder().Build();
            var sut = new SalesInvoicePdfServiceBuilder()
                .WithReader(new SalesInvoiceReaderStubBuilder().WithReadPdfOk(invoice).Build())
                .WithFormat(format)
                .Build();

            var actual = await sut.ToImportFormat(Array.Empty<byte>());

            var expected = new List<AccountingEntryLine>()
            {
                new Debit(new(1700), new("Counter transaction"), 124m, null, VatType.Undefined),
                new Credit(new(3000), new("petri.works"), 100m, new(24), VatType.Sale)
            };
            format.Received(1).Format(Arg.Is<AccountingEntry>(value =>
                value.GetLines().SequenceEqual(expected)
            ));
        }


        [Test]
        public async Task Importing_WithReadingInvoiceNotHavingSameTotalAndLineSums_ReturnsError()
        {
            var invoice = new SalesInvoiceBuilder()
                .WithInvoiceTotal(24m)
                .WithLine(new SalesInvoiceLineBuilder().WithName("petri.works").WithAmount(1).WithUnitNetPrice(100m).WithVatPercentage(24));
            var format = new AccountingEntryFormatStubBuilder().Build();
            var sut = new SalesInvoicePdfServiceBuilder()
                .WithReader(new SalesInvoiceReaderStubBuilder().WithReadPdfOk(invoice).Build())
                .WithFormat(format)
                .Build();

            var actual = await sut.ToImportFormat(Array.Empty<byte>());

            Assert.IsFalse(actual.IsOk);
            Assert.AreEqual("Can't create accounting entry when lines total sum is -100,00", actual.Error);
        }

        [Test]
        public async Task Importing_WithGivenPdfFile_CallsReaderCorrectly()
        {
            var expected = new byte[] { 0, 16, 104, 213 };
            var reader = new SalesInvoiceReaderStubBuilder().Build();
            var sut = new SalesInvoicePdfServiceBuilder().WithReader(reader).Build();

            await sut.ToImportFormat(expected);

            await reader.Received(1).ReadPdf(expected);
        }

        [Test]
        public async Task Importing_WithFormatter_ReturnsFormattedAccountingEntry()
        {
            var format = new AccountingEntryFormatStubBuilder().WithFormat("formatted").Build();
            var sut = new SalesInvoicePdfServiceBuilder().WithFormat(format).Build();

            var actual = await sut.ToImportFormat(Array.Empty<byte>());

            Assert.IsTrue(actual.IsOk);
            Assert.AreEqual("formatted", actual.Value);
        }
    }
}
