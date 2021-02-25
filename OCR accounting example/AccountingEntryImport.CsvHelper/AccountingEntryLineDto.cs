using CsvHelper.Configuration.Attributes;

namespace AccountingEntryImport.CsvHelper
{
    internal class AccountingEntryLineDto
    {
        [Name("Account number")]
        public int AccountNumber { get; set; }
        [Name("Product")]
        public string? ProductName { get; set; }
        public decimal Sum { get; set; }
        public string? Dimension { get; set; }
        [Name("Dimension item")]
        public string? DimensionItem { get; set; }
        [Name("VAT %")]
        public int? VatPercentage { get; set; }
        [Name("VAT type")]
        public string? VatType { get; set; }
    }
}
