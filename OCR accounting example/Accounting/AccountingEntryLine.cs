using System;

namespace Accounting
{
    public record AccountingEntryLine(AccountNumber Number, ProductName Name, decimal NetSum, FinnishVatPercentage? Vat)
    {
        public decimal GrossSum() => Vat == null ? NetSum : NetSum * Vat.GetMultiplier();
    }

    public record Credit(AccountNumber Number, ProductName Name, decimal NetSum, FinnishVatPercentage? Vat): 
        AccountingEntryLine(Number, Name, -Math.Abs(NetSum), Vat);

    public record Debit(AccountNumber Number, ProductName Name, decimal NetSum, FinnishVatPercentage? Vat) :
        AccountingEntryLine(Number, Name, Math.Abs(NetSum), Vat);
}
