using Builders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Tests
{
    class DebitTests
    {
        [Test]
        public void Creating_WithPositiveValue_SetsItAsPositive()
        {
            var sut = new DebitBuilder().WithNetSum(100).Build();

            var actual = sut.NetSum;

            Assert.AreEqual(100m, actual);
        }

        [Test]
        public void Creating_WithNegativeValue_SetsItAsPositive()
        {
            var sut = new DebitBuilder().WithNetSum(-100).Build();

            var actual = sut.NetSum;

            Assert.AreEqual(100m, actual);
        }
    }
}
