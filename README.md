# OCR accounting example

## Prerequisites
You need to install following software to being able to build and run this example:
- Visual Studio: https://visualstudio.microsoft.com/downloads/
 - Install with:
  - .NET Core cross-platform development
- .NET 5 SDK: https://dotnet.microsoft.com/download/dotnet/5.0
- Docker
 - Windows: https://docs.docker.com/docker-for-windows/install/
 - Mac: https://docs.docker.com/docker-for-mac/install/

## IronOcr
IronOcr offers free version for development purposes only. This creates a restriction that app can only be debugged if license key is not added. Because of this, this example with SalesInvoiceImport.IronOcr implementation can only be debugged in Visual Studio. This same also applies for running tests for SalesInvoiceImport.IronOcr.Tests.

## Design
This has been designed to be modular implementation in which we can easily swap sales invoice reader to some other implementation, for example using GCP Document AI or Amazon Textract. Just create new library and implement required interface (ISalesInvoiceReader).

Same also applies for output format, currently we are supporting CSV but it can be changed for example to JSON or XML just by creating new library and implementing required interface (IAcccountingEntryFormat).

Accounting contains business logic related for accounting domain. It handles creating accounting entries from sales invoices. It is not concerned with technical difficulties, for example CSV formats or PDF OCRing, and is only focusing on business logic.

Currently we have only single example place that is using this logic, SalesInvoiceToAccountingImport, which also handles logging aspect. This way logging is centralized in single place and is easy to undestand. Also it is not tangled with other technical difficulties or business logic.

## How to run
### Console app
Open solutions file in Visual Studio. Select "SalesInvoiceToAccountingImport" as startupt project. Then debug project. This will be debugged in Docker image.
### Unit Tests
Open solutions file in Visual Studio. Following projects can be executed normally:
- Accounting.Tests
- AccountingEntryImport.CsvHelper.Tests

SalesInvoiceImport.IronOcr.Tests requires special attention because of unlicensed IronOcr library. Run these tests in debug mode.