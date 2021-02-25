using Builders;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Accounting.Tests
{
    class AccountingEntryTests
    {
        [Test]
        public void Creating_WithoutLineSumsBeingZero_ThrowsException()
        {
            var credit = new CreditBuilder().WithNetSum(100).WithVat(24).Build();
            var debit = new DebitBuilder().WithNetSum(10).WithVat(10).Build();
            var sut = new AccountingEntryBuilder().WithLine(credit).WithLine(debit);

            Assert.Throws<ArgumentException>(() => sut.Build());
        }

        [Test]
        public void Creating_WithoutLines_ReturnsEmptyList()
        {
            var sut = new AccountingEntryBuilder().Build();

            var actual = sut.GetLines();

            CollectionAssert.IsEmpty(actual);
        }

        [Test]
        public void AskingLines_WithLinesHavingDifferentVats_OrdersThemCorrectly()
        {
            var vat24 = new CreditBuilder().WithNetSum(100).WithVat(24).Build();
            var vat14 = new CreditBuilder().WithNetSum(100).WithVat(14).Build();
            var vat10 = new CreditBuilder().WithNetSum(100).WithVat(10).Build();
            var vat0 = new CreditBuilder().WithNetSum(100).WithVat(0).Build();
            var noVat = new DebitBuilder().WithNetSum(448).WithVat(null).Build();
            var sut = new AccountingEntryBuilder()
                .WithLine(vat14)
                .WithLine(vat24)
                .WithLine(vat0)
                .WithLine(noVat)
                .WithLine(vat10)
                .Build();

            var actual = sut.GetLines();

            var expected = new List<AccountingEntryLine>() { noVat, vat0, vat10, vat14, vat24 };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
