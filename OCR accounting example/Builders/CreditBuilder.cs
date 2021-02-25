using Accounting;

namespace Builders
{
    public class CreditBuilder
    {
        private AccountNumber number = new(2000);
        private ProductName name = new("product");
        private decimal netSum = 100;
        private FinnishVatPercentage? vat = new(24);
        private VatType vatType = VatType.Purchase;

        public CreditBuilder WithAccountNumber(int value)
        {
            number = new(value);
            return this;
        }

        public CreditBuilder WithProductName(string value)
        {
            name = new(value);
            return this;
        }

        public CreditBuilder WithNetSum(decimal value)
        {
            netSum = value;
            return this;
        }

        public CreditBuilder WithVat(int? value)
        {
            if (value.HasValue)
            {
                vat = new(value.Value);
            }
            else
            {
                vat = null;
            }

            return this;
        }

        public CreditBuilder WithVatType(VatType value)
        {
            vatType = value;
            return this;
        }

        public Credit Build()
        {
            return new(number, name, netSum, vat, vatType);
        }

    }
}
