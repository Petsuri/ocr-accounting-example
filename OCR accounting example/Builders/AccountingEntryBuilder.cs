using Accounting;
using System.Collections.Generic;

namespace Builders
{
    public class AccountingEntryBuilder
    {
        private readonly List<AccountingEntryLine> lines = new();

        public AccountingEntryBuilder WithLine(AccountingEntryLine value)
        {
            lines.Add(value);
            return this;
        }

        public AccountingEntry Build()
        {
            return new(lines);
        }
    }
}
