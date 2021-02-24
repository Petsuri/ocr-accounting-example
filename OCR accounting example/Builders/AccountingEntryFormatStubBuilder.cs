using Accounting;
using NSubstitute;

namespace Builders
{
    public class AccountingEntryFormatStubBuilder
    {
        private string format = "";

        public AccountingEntryFormatStubBuilder WithFormat(string value)
        {
            format = value;
            return this;
        }

        public IAccountingEntryFormat Build()
        {
            var stub = Substitute.For<IAccountingEntryFormat>();
            stub.Format(Arg.Any<AccountingEntry>()).Returns(format);

            return stub;
        }
    }
}
