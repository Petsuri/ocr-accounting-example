using Builders;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesInvoiceImport.IronOcr.Tests
{
    class IronOcrSalesInvoiceReaderTests
    {
        [Test]
        public async Task Reading_WithEmptydPdf_ReturnsError()
        {
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = await sut.ReadPdf(Array.Empty<byte>());

            Assert.IsFalse(actual.IsOk);
            Assert.AreEqual("Empty byte array is not valid PDF file", actual.Error);
        }

        [Test]
        public async Task Reading_WithInvaliddPdf_ReturnsError()
        {
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = await sut.ReadPdf(new byte[] { 0x20 });

            Assert.IsFalse(actual.IsOk);
            StringAssert.Contains("Can not parse a PDF", actual.Error);
            StringAssert.Contains("Please check that this is a valid PDF file", actual.Error);
        }

        [Test]
        public async Task Reading_WithValidPdfNotBeingSalesInvoice_ReturnsError()
        {
            var emptyPdf = File.ReadAllBytes(TestContext.CurrentContext.TestDirectory + "/TestPdfs/empty.pdf");
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = await sut.ReadPdf(emptyPdf);

            Assert.IsFalse(actual.IsOk);
            StringAssert.Contains("Sales invoice information can't be found from PDF", actual.Error);
            StringAssert.Contains("Please check that this is a valid sales invoice", actual.Error);
        }

        [Test]
        public async Task Reading_WithSalesInvoiceHavingFourLines_FindsAllFourLines()
        {
            var emptyPdf = File.ReadAllBytes(TestContext.CurrentContext.TestDirectory + "/TestPdfs/sales-invoice-with-4-lines.pdf");
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = await sut.ReadPdf(emptyPdf);

            Assert.IsTrue(actual.IsOk);
            Assert.AreEqual(4, actual.Value?.Lines.Count);
        }

        [Test]
        public async Task Reading_WithSalesInvoiceHavingSingleLines_FindsSingleLine()
        {
            var emptyPdf = File.ReadAllBytes(TestContext.CurrentContext.TestDirectory + "/TestPdfs/sales-invoice-with-1-lines.pdf");
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = await sut.ReadPdf(emptyPdf);

            Assert.IsTrue(actual.IsOk);
            Assert.AreEqual(1, actual.Value?.Lines.Count);
        }

        [Test]
        public async Task Reading_WithSalesInvoiceHavingFourLines_ReturnsExpectedTotald()
        {
            var emptyPdf = File.ReadAllBytes(TestContext.CurrentContext.TestDirectory + "/TestPdfs/sales-invoice-with-4-lines.pdf");
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = await sut.ReadPdf(emptyPdf);

            Assert.AreEqual(5826.52m, actual.Value?.InvoiceTotal);
        }

        [Test]
        public async Task Reading_WithSalesInvoiceHavingSingleLine_ReturnsExpectedTotal()
        {
            var emptyPdf = File.ReadAllBytes(TestContext.CurrentContext.TestDirectory + "/TestPdfs/sales-invoice-with-1-lines.pdf");
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = await sut.ReadPdf(emptyPdf);

            Assert.AreEqual(12400m, actual.Value?.InvoiceTotal);
        }

        [Test]
        public async Task Reading_WithSalesInvoiceHavingSingleLine_ParsesLineInformationCorrectly()
        {
            var emptyPdf = File.ReadAllBytes(TestContext.CurrentContext.TestDirectory + "/TestPdfs/sales-invoice-with-1-lines.pdf");
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = (await sut.ReadPdf(emptyPdf)).Value.Lines.Single();

            Assert.AreEqual("Consulting", actual.Name.Value);
            Assert.AreEqual(1, actual.Amount);
            Assert.AreEqual("pcs", actual.Unit.Value);
            Assert.AreEqual(10000m, actual.UnitNetPrice);
            Assert.AreEqual(24, actual.VatPercentage.Value);
        }

        [Test]
        public async Task Reading_WithSalesInvoiceHavingFourLines_ParsesFirstLineInformationCorrectly()
        {
            var emptyPdf = File.ReadAllBytes(TestContext.CurrentContext.TestDirectory + "/TestPdfs/sales-invoice-with-4-lines.pdf");
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = (await sut.ReadPdf(emptyPdf)).Value.Lines.First();

            Assert.AreEqual("Product1", actual.Name.Value);
            Assert.AreEqual(1, actual.Amount);
            Assert.AreEqual("pcs", actual.Unit.Value);
            Assert.AreEqual(100m, actual.UnitNetPrice);
            Assert.AreEqual(24, actual.VatPercentage.Value);
        }

        [Test]
        public async Task Reading_WithSalesInvoiceHavingFourLines_ParsesLastLineInformationCorrectly()
        {
            var emptyPdf = File.ReadAllBytes(TestContext.CurrentContext.TestDirectory + "/TestPdfs/sales-invoice-with-4-lines.pdf");
            var sut = new IronOcrSalesInvoiceReaderBuilder().Build();

            var actual = (await sut.ReadPdf(emptyPdf)).Value?.Lines.Last();

            Assert.AreEqual("Product4", actual.Name.Value);
            Assert.AreEqual(1, actual.Amount);
            Assert.AreEqual("pcs", actual.Unit.Value);
            Assert.AreEqual(5567m, actual.UnitNetPrice);
            Assert.AreEqual(0, actual.VatPercentage.Value);
        }
    }
}
