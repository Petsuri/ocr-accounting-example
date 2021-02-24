using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting
{
    public class SalesInvoicePdfService
    {
        private readonly ISalesInvoiceReader reader;
        private readonly IAccountingEntryFormat formatter;

        public SalesInvoicePdfService(ISalesInvoiceReader reader, IAccountingEntryFormat formatter)
        {
            this.reader = reader;
            this.formatter = formatter;
        }

        public async Task<Result<string>> ToImportFormat(byte[] pdf)
        {
            var invoiceResult = await reader.ReadPdf(pdf);
            if (invoiceResult.Value == null)
            {
                return Result<string>.Failure(invoiceResult.Error);
            }

            var accountingEntry = ToAccounting(invoiceResult.Value);
            var formatted = formatter.Format(accountingEntry);
            return Result<string>.Ok(formatted);
        }

        private static AccountingEntry ToAccounting(SalesInvoice invoice)
        {
            var debit = new Debit(new(1700), new ProductName("Counter transaction"), invoice.InvoiceTotal, null);
            List<AccountingEntryLine> accountingLines = invoice.Lines
                .Select(ToCredit)
                .ToList();
            accountingLines.Add(debit);

            return new(accountingLines);
        }

        private static AccountingEntryLine ToCredit(SalesInvoiceLine line)
        {
            return new Credit(new(3000), line.Name, line.Amount * line.UnitNetPrice, line.VatPercentage);
        }
    }
}
