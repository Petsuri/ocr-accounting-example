using SalesInvoiceImport.IronOcr;
using System.Collections.Generic;

namespace Builders
{
    public class SalesInvoiceTotalSumBuilder
    {
        private readonly List<IInvoiceTotalSum> totalSums;

        public SalesInvoiceTotalSumBuilder()
        {
            totalSums = new()
            {
                new SameLineInvoiceTotalSum(),
                new SameHeightInvoiceTotalSum(),
            };
        }

        public SalesInvoiceTotalSum Build()
        {
            return new SalesInvoiceTotalSum(totalSums);
        }

        public static implicit operator SalesInvoiceTotalSum(SalesInvoiceTotalSumBuilder b)
        {
            return b.Build();
        }

    }
}
