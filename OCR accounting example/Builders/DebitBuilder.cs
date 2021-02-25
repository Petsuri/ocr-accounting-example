using Accounting;

namespace Builders
{
    public class DebitBuilder
    {
        private AccountNumber number = new(2000);
        private ProductName name = new("product");
        private decimal netSum = 100;
        private FinnishVatPercentage? vat = new(24);
        private VatType vatType = VatType.Purchase;

        public DebitBuilder WithAccountNumber(int value)
        {
            number = new(value);
            return this;
        }

        public DebitBuilder WithProductName(string value)
        {
            name = new(value);
            return this;
        }

        public DebitBuilder WithNetSum(decimal value)
        {
            netSum = value;
            return this;
        }

        public DebitBuilder WithVat(int? value)
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

        public DebitBuilder WithVatType(VatType value)
        {
            vatType = value;
            return this;
        }

        public Debit Build()
        {
            return new(number, name, netSum, vat, vatType);
        }
    }
}
