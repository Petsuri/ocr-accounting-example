using System;
using System.Collections.Generic;
using System.Linq;

namespace Accounting
{
    public class AccountingEntry
    {
        private const int FirstOrder = -1;

        private readonly IReadOnlyList<AccountingEntryLine> lines;

        public AccountingEntry(IEnumerable<AccountingEntryLine> lines)
        {
            if (!IsValid(lines))
            {
                throw new ArgumentException($"Sum of accounting entry lines must be zero. Sum was: {GetTotalSum(lines)}", nameof(lines));
            }
            

            this.lines = lines.ToList();
        }

        public static bool IsValid(IEnumerable<AccountingEntryLine> lines)
        {
            return GetTotalSum(lines) == decimal.Zero;
        }

        public static decimal GetTotalSum(IEnumerable<AccountingEntryLine> lines)
        {
            return lines.Sum(line => line.GrossSum());
        }

        public IReadOnlyList<AccountingEntryLine> GetLines()
        {
            return lines
                .OrderBy(AscendingVatPercent)
                .ToList();
        }

        private static int AscendingVatPercent(AccountingEntryLine line)
        {
            return line.Vat == null ? FirstOrder : line.Vat.Value;
        }
    }
}
