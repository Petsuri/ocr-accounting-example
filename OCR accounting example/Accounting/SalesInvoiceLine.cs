namespace Accounting
{
    public class SalesInvoiceLine
    {
        public string ProductName { get; }
        public int Amount { get; }
        public string Unit { get; }
        public decimal UnitPrice { get; }
        public FinnishVatPercentage VatPercentage { get; }

        public SalesInvoiceLine(string productName, int amount, string unit, decimal unitPrice, FinnishVatPercentage vatPercentage)
        {
            ProductName = productName;
            Amount = amount;
            Unit = unit;
            UnitPrice = unitPrice;
            VatPercentage = vatPercentage;
        }
    }
}
