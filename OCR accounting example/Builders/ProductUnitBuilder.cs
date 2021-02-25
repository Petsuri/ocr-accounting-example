using Accounting;

namespace Builders
{
    public class ProductUnitBuilder
    {
        private string value = "pcs";

        public ProductUnitBuilder WithValue(string value)
        {
            this.value = value;
            return this;
        }

        public ProductUnit Build()
        {
            return new(value);
        }
    }
}
