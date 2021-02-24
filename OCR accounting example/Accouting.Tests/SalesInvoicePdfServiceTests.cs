using Accounting;
using Builders;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accouting.Tests
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
        public async Task Importing_WithReadingPdfSuccessfully_CreatesCorrectPdfEntry()
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

            var expectedDebit = new Debit(new(1700), new("Counter transaction"), 124m, null);
            var expectedCredit = new Credit(new(3000), new("petri.works"), 100m, new(24));
            var expected = new List<AccountingEntryLine>()
            {
                new Debit(new(1700), new("Counter transaction"), 124m, null),
                
            };
            format.Received(1).Format(Arg.Is<AccountingEntry>(value =>
                CreditsEquals(value.GetLines().Last(), expectedCredit)
            ));
        }

        private bool CreditsEquals(AccountingEntryLine first, AccountingEntryLine second)
        {
            return first.Equals(second);
        }
    }
}
