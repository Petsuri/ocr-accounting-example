using Builders;
using NUnit.Framework;
using System;

namespace Accounting.Tests
{
    class FinnishVatPercentageTests
    {
        [TestCase(-1)]
        [TestCase(25)]
        public void Creating_WithInvalidVat_ThrowsException(int vat)
        {
            var sut = new FinnishVatPercentageBuilder().WithValue(vat);

            Assert.IsFalse(FinnishVatPercentage.IsValid(vat));
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Build());
        }

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(14)]
        [TestCase(24)]
        public void Creating_WithValidVat_IsExpected(int expected)
        {
            var sut = new FinnishVatPercentageBuilder().WithValue(expected).Build();

            var actual = sut.Value;

            Assert.AreEqual(expected, actual);
        }
    }
}
