using System;
using System.Collections.Generic;
using System.Linq;

namespace Accounting
{
    public sealed class ProductUnit
    {
        private static readonly IReadOnlyList<string> ProductUnits = new List<string>()
        {
            "pcs"
        };

        public string Value { get; }

        public ProductUnit(string value)
        {
            if (!IsValid(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Is not valid product unit");
            }

            Value = value;
        }

        public static bool IsValid(string value)
        {
            return ProductUnits.Contains(value);
        }
    }
}
