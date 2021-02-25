using System;

namespace Accounting
{
    public enum VatType
    {
        Undefined = 1,
        Purchase = 2,
        Sale = 3,
    };

    public abstract record AccountingEntryLine(AccountNumber Number, ProductName Name, decimal NetSum, FinnishVatPercentage? Vat, VatType VatType)
    {
        public decimal GrossSum() => Vat == null ? NetSum : NetSum * Vat.GetMultiplier();
        public bool HasVatPercentage() => GetVatPercentage() != null;
        public int? GetVatPercentage() => Vat == null ? null : Vat.Value;
    }

    public record Credit(AccountNumber Number, ProductName Name, decimal NetSum, FinnishVatPercentage? Vat, VatType VatType) : 
        AccountingEntryLine(Number, Name, -Math.Abs(NetSum), Vat, VatType);

    public record Debit(AccountNumber Number, ProductName Name, decimal NetSum, FinnishVatPercentage? Vat, VatType VatType) :
        AccountingEntryLine(Number, Name, Math.Abs(NetSum), Vat, VatType);
}
