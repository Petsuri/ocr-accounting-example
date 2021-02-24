using Builders;
using NUnit.Framework;
using System;
using System.IO;
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
    }
}
