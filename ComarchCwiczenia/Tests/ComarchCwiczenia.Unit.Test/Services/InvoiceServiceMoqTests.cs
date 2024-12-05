using ComarchCwiczenia.Services;
using FluentAssertions;
using Moq;

namespace ComarchCwiczenia.Unit.Test.Services;

[TestFixture]
public class InvoiceServiceMoqTests
{
    private IInvoiceService invoiceService;
    private Mock<ITaxService> taxServiceMock;
    private Mock<IDiscountService> discountServiceMock;

    [SetUp]
    public void Setup()
    {
        taxServiceMock = new Mock<ITaxService>();
        discountServiceMock = new Mock<IDiscountService>();

        invoiceService = new InvoiceService(taxServiceMock.Object, discountServiceMock.Object);
    }

    [Test]
    public void CalculateTotalWhenCalledVerifiesTaxServiceGetTaxIsCalled()
    {
        // Arrange
        decimal amount = 100m;
        string customerType = "Regular";

        discountServiceMock.Setup(ds => ds.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        taxServiceMock.Setup(ts => ts.GetTax(It.IsAny<decimal>())).Returns(5).Verifiable();


        // Act
        invoiceService.CalculateTotal(amount, customerType);

        // Assert
        taxServiceMock.Verify(ts => ts.GetTax(It.IsAny<decimal>()), Times.Once);
    }

    [Test]
    public void CalculateTotalShouldReturnsExpectedTotal()
    {
        // Arrange
        decimal amount = 100m;
        string customerType = "Regular";

        discountServiceMock.Setup(ds => ds.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        taxServiceMock.Setup(ts => ts.GetTax(It.IsAny<decimal>())).Returns(5);


        // Act
        decimal actual = invoiceService.CalculateTotal(amount, customerType);

        // Assert
        actual.Should().Be(95m);
    }

    [Test]
    public void CalculateTotalShouldUseCallbackToManipulateAmount()
    {
        //Arrange
        decimal capturedDiscount = 0;

        discountServiceMock.Setup(ds => ds.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10m)
            .Callback<decimal, string>((amount, customerType) => capturedDiscount = amount * 0.1m);

        // Act
        invoiceService.CalculateTotal(100m, "Regular");

        //Assert
        capturedDiscount.Should().Be(10m);
    }
}