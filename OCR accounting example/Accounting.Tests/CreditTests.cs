using Builders;
using NUnit.Framework;

namespace Accounting.Tests
{
    class CreditTests
    {
        [Test]
        public void Creating_WithPositiveValue_SetsItAsNegative()
        {
            var sut = new CreditBuilder().WithNetSum(100).Build();

            var actual = sut.NetSum;

            Assert.AreEqual(-100m, actual);
        }

        [Test]
        public void Creating_WithNegativeValue_SetsItAsNegative()
        {
            var sut = new CreditBuilder().WithNetSum(-100).Build();

            var actual = sut.NetSum;

            Assert.AreEqual(-100m, actual);
        }

        [TestCase(null, -100)]
        [TestCase(0, -100)]
        [TestCase(10, -110)]
        [TestCase(14, -114)]
        [TestCase(24, -124)]
        public void AskingGrossSum_WithDifferentVats_IsExpected(int? vatPercentage, decimal expected)
        {
            var sut = new CreditBuilder().WithNetSum(100).WithVat(vatPercentage).Build();

            var actual = sut.GrossSum();

            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, null)]
        [TestCase(0, 0)]
        [TestCase(10, 10)]
        [TestCase(14, 14)]
        [TestCase(24, 24)]
        public void AskingVatPercentage_WithDifferentVats_IsExpected(int? vatPercentage, int? expected)
        {
            var sut = new CreditBuilder().WithNetSum(100).WithVat(vatPercentage).Build();

            var actual = sut.GetVatPercentage();

            Assert.AreEqual(expected, actual);
        }
    }
}
