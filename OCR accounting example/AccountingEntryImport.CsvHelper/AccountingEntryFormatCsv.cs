using Accounting;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AccountingEntryImport.CsvHelper
{
    public class AccountingEntryFormatCsv : IAccountingEntryFormat
    {
        private static readonly IReadOnlyDictionary<VatType, string?> VatTypeMappings = new Dictionary<VatType, string?>()
        {
            { VatType.Undefined, null },
            { VatType.Purchase, "P" },
            { VatType.Sale, "S" },
        };

        public string Format(AccountingEntry entry)
        {
            var dtos = entry
                .GetLines()
                .Select(ToDto)
                .ToList();

            var csvBytes = ToCsv(dtos);
            return Encoding.UTF8.GetString(csvBytes);
        }

        private static AccountingEntryLineDto ToDto(AccountingEntryLine line)
        {
            return new AccountingEntryLineDto()
            {
                AccountNumber = line.Number.Value,
                ProductName = line.Name.Value,
                Sum = line.NetSum,
                Dimension = null,
                DimensionItem = null,
                VatPercentage = line.GetVatPercentage(),
                VatType = VatTypeMappings[line.VatType],
            };
        }

        private static byte[] ToCsv(IReadOnlyList<AccountingEntryLineDto> lines)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            using var csvWriter = new CsvWriter(streamWriter, GetConfiguration());
            {
                csvWriter.WriteRecords(lines);
                csvWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream.ToArray();
            }
        }

        private static CsvConfiguration GetConfiguration()
        {
            var finnishCulture = new CultureInfo("fi-FI");
            finnishCulture.NumberFormat = new()
            {
                NegativeSign = "-",
                NumberDecimalSeparator = ",",
            };
            return new CsvConfiguration(finnishCulture)
            {
                Delimiter = ";",
                Encoding = Encoding.UTF8, 
            };
        }
    }
}
