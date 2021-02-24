namespace Accounting
{
    public sealed class SalesInvoiceLine
    {
        public ProductName Name { get; }
        public int Amount { get; }
        public ProductUnit Unit { get; }
        public decimal UnitNetPrice { get; }
        public FinnishVatPercentage VatPercentage { get; }

        public SalesInvoiceLine(ProductName name, int amount, ProductUnit unit, decimal unitNetPrice, FinnishVatPercentage vatPercentage)
        {
            Name = name;
            Amount = amount;
            Unit = unit;
            UnitNetPrice = unitNetPrice;
            VatPercentage = vatPercentage;
        }
    }
}
