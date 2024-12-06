using AutoFixture;
using AutoFixture.AutoMoq;
using ComarchCwiczenia.Services;
using ComarchCwiczenia.Services.Model;
using Moq;
using NUnit.Framework.Constraints;

namespace ComarchCwiczenia.Unit.Test.Services;

public class InvoiceServiceAutofixtureMoqTests
{
    private IFixture fixture;
    private Mock<IInvoiceRepository> invoiceRepositoryServiceMock;
    private Mock<IEmailSender> emailSenderServiceMock;
    private InvoiceService invoiceService;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoMoqCustomization());
        invoiceRepositoryServiceMock = fixture.Freeze<Mock<IInvoiceRepository>>();
        emailSenderServiceMock = fixture.Freeze<Mock<IEmailSender>>();
        invoiceService = fixture.Create<InvoiceService>();
    }

    [Test]
    public void CreateInvoiceShouldSaveInvoiceAndSendEmail()
    {
        // Arrange 
        Invoice invoice = fixture.Create<Invoice>();

        // Act
        invoiceService.ProceedInvoice(invoice);

        //Assert
        invoiceRepositoryServiceMock.Verify(r => r.Save(invoice), Times.Once);
        emailSenderServiceMock.Verify(e => e.Send(invoice.CustomerEmail, It.IsAny<string>(), It.IsAny<string>()));
    }
}