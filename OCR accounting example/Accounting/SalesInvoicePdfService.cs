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
            if (!accountingEntry.IsOk)
            {
                return Result<string>.Failure(accountingEntry.Error);
            }

            var formatted = formatter.Format(accountingEntry.Value);
            return Result<string>.Ok(formatted);
        }

        private static Result<AccountingEntry> ToAccounting(SalesInvoice invoice)
        {
            var debit = new Debit(new(1700), new ProductName("Counter transaction"), invoice.InvoiceTotal, null, VatType.Undefined);
            List<AccountingEntryLine> accountingLines = invoice.Lines
                .Select(ToCredit)
                .ToList();
            accountingLines.Add(debit);

            if (AccountingEntry.IsValid(accountingLines))
            {
                return Result<AccountingEntry>.Ok(new(accountingLines));
            }

            return Result<AccountingEntry>.Failure($"Can't create accounting entry when lines total sum is {AccountingEntry.GetTotalSum(accountingLines)}");
        }

        private static AccountingEntryLine ToCredit(SalesInvoiceLine line)
        {
            return new Credit(new(3000), line.Name, line.Amount * line.UnitNetPrice, line.VatPercentage, VatType.Sale);
        }
    }
}
