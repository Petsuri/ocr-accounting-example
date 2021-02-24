namespace Accounting
{
    public sealed class SalesInvoiceLine
    {
        public string ProductName { get; }
        public int Amount { get; }
        public ProductUnit Unit { get; }
        public decimal UnitNetPrice { get; }
        public FinnishVatPercentage VatPercentage { get; }

        public SalesInvoiceLine(string productName, int amount, ProductUnit unit, decimal unitNetPrice, FinnishVatPercentage vatPercentage)
        {
            ProductName = productName;
            Amount = amount;
            Unit = unit;
            UnitNetPrice = unitNetPrice;
            VatPercentage = vatPercentage;
        }
    }
}
