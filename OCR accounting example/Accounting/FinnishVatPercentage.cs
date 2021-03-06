﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Accounting
{
    public sealed record FinnishVatPercentage
    {
        private static readonly IReadOnlyList<int> FinnishVatPercentages = new List<int>()
        {
            0, 10, 14, 24
        };

        public int Value { get; }

        public decimal GetMultiplier() => 1 + Value / 100m;

        public FinnishVatPercentage(int value)
        {
            if (!IsValid(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Is not valid Finnish vat percentage");
            }

            Value = value;
        }

        public static bool IsValid(int value)
        {
            return FinnishVatPercentages.Contains(value);
        }

    }
}
