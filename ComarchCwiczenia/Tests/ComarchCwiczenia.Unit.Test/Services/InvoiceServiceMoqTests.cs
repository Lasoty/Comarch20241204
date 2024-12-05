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
    private Mock<IOrderProvider> orderProviderMock;

    [SetUp]
    public void Setup()
    {
        taxServiceMock = new Mock<ITaxService>();
        discountServiceMock = new Mock<IDiscountService>();
        orderProviderMock = new Mock<IOrderProvider>();

        invoiceService = new InvoiceService(taxServiceMock.Object, discountServiceMock.Object, orderProviderMock.Object);
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

    [Test]
    public void CalculateInvoiceAmount_UsesTaxAndDiscountServices()
    {
        // Arrange
        discountServiceMock.Setup(ds => ds.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        taxServiceMock.Setup(ts => ts.GetTax(It.IsAny<decimal>())).Returns(5).Verifiable();
        orderProviderMock.Setup(os => os.GetOrderAmount(It.IsAny<int>())).Returns(100).Verifiable();
        
        // Act
        var result = invoiceService.CalculateInvoiceAmount(1, "Regular");
        // Assert
        result.Should().Be(95); // 100 - 10 + 5
        discountServiceMock.Verify(s => s.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once);
        taxServiceMock.Verify(s => s.GetTax(90), Times.Once); //Możemy określić ilość wywołań danej metody dla konkretnej wartości parametru amount.
    }
}