using IronOcr;
using System.Collections.Generic;

namespace SalesInvoiceImport.IronOcr
{
    public interface IInvoiceTotalSum
    {
        decimal? Find(IReadOnlyList<OcrResult.Line> lines);
    }
}
