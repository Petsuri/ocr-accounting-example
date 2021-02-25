using Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builders
{
    public class FinnishVatPercentageBuilder
    {
        private int value = 24;

        public FinnishVatPercentageBuilder WithValue(int value)
        {
            this.value = value;
            return this;
        }

        public FinnishVatPercentage Build()
        {
            return new(value);
        }
    }
}
