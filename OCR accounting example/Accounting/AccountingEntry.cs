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
            var listOfLines = lines.ToList();
            var totalSum = listOfLines.Sum(line => line.GrossSum());
            if (totalSum != decimal.Zero)
            {
                throw new ArgumentException($"Sum of accounting entry lines must be zero. Sum was: {totalSum}", nameof(lines));
            }
            

            this.lines = listOfLines;
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
