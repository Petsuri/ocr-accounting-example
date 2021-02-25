using Builders;
using NUnit.Framework;
using System;

namespace Accounting.Tests
{
    class ProductUnitTests
    {
        [TestCase("xxx")]
        [TestCase("PCS")]
        public void Creating_WithInvalidUnit_ThrowsException(string unit)
        {
            var sut = new ProductUnitBuilder().WithValue(unit);

            Assert.IsFalse(ProductUnit.IsValid(unit));
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Build());
        }

        [TestCase("pcs")]
        public void Creating_WithValidVat_IsExpected(string expected)
        {
            var sut = new ProductUnitBuilder().WithValue(expected).Build();

            var actual = sut.Value;

            Assert.AreEqual(expected, actual);
        }
    }
}
