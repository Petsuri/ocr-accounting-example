using Accounting;
using Builders;
using NUnit.Framework;

namespace AccountingEntryImport.CsvHelper.Tests
{
    class AccountingEntryFormatCsvTests
    {
        [Test]
        public void Formatting_WithEntryHavingSingleDebitAndCredit_IsExpected()
        {
            var entry = new AccountingEntryBuilder()
                .WithLine(new CreditBuilder()
                .WithAccountNumber(3000)
                .WithProductName("Credit")
                .WithNetSum(1000)
                .WithVat(24)
                .WithVatType(VatType.Sale)
                .Build())
                .WithLine(new DebitBuilder()
                .WithAccountNumber(1700)
                .WithProductName("Debit")
                .WithNetSum(1240)
                .WithVat(null)
                .WithVatType(VatType.Undefined)
                .Build())
                .Build();
            var sut = new AccountingEntryFormatCsvBuilder().Build();

            var actual = sut.Format(entry);

            var expected = @"
Account number;Product;Sum;Dimension;Dimension item;VAT %;VAT type
1700;Debit;1240;;;;
3000;Credit;-1000;;;24;S
".TrimStart();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Formatting_WithEntryHavingMultipleLines_IsExpected()
        {
            var entry = new AccountingEntryBuilder()
                .WithLine(new CreditBuilder()
                .WithAccountNumber(3100)
                .WithProductName("Product1")
                .WithNetSum(33)
                .WithVat(14)
                .WithVatType(VatType.Sale)
                .Build())
                .WithLine(new CreditBuilder()
                .WithAccountNumber(3200)
                .WithProductName("Product2")
                .WithNetSum(89)
                .WithVat(10)
                .WithVatType(VatType.Purchase)
                .Build())
                .WithLine(new DebitBuilder()
                .WithAccountNumber(1600)
                .WithProductName("Counter Transaction")
                .WithNetSum(135.52m)
                .WithVat(null)
                .WithVatType(VatType.Undefined)
                .Build())
                .Build();
            var sut = new AccountingEntryFormatCsvBuilder().Build();

            var actual = sut.Format(entry);

            var expected = @"
Account number;Product;Sum;Dimension;Dimension item;VAT %;VAT type
1600;Counter Transaction;135,52;;;;
3200;Product2;-89;;;10;P
3100;Product1;-33;;;14;S
".TrimStart();
            Assert.AreEqual(expected, actual);
        }
    }
}
