namespace Accounting
{
    public interface IAccountingEntryFormat
    {
        string Format(AccountingEntry entry);
    }
}
