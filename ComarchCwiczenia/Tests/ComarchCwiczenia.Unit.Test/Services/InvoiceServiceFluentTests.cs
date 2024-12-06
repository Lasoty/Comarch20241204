using ComarchCwiczenia.Services;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace ComarchCwiczenia.Unit.Test.Services;

[TestFixture]
public class InvoiceServiceFluentTests
{
    private InvoiceService cut;

    [SetUp]
    public void Setup()
    {
        cut = new InvoiceService(null, null);
    }

    #region String tests
    [Test]
    public void GenerateInvoiceNumberShouldStartWithINV()
    {
        // Act
        string invoiceNumber = cut.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().StartWith("INV-");
    }

    [Test]
    public void GenerateInvoiceNumberShouldEndWithNumericSuffix()
    {
        // Act
        string invoiceNumber = cut.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().MatchRegex(@"INV-\d{8}-\d{3}$");
    }

    [Test]
    public void GenerateInvoiceNumberShouldContainCurrentDate()
    {
        // Arrange
        string currentDate = DateTime.Now.ToString("yyyyMMdd");

        // Act
        string invoiceNumber = cut.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().Contain(currentDate);
    }

    [Test]
    public void GenerateInvoiceNumberShouldHaveLengthOf16()
    {
        // Act
        string invoiceNumber = cut.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().HaveLength(16);
    }

    [Test]
    public void GenerateInvoiceNumberShouldNotBeEmpty()
    {
        // Act
        var invoiceNumber = cut.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void GenerateInvoiceNumberShouldMachExpectedFormat()
    {
        var invoiceNumber = cut.GenerateInvoiceNumber();
        invoiceNumber.Should().Match("INV-????????-???");
    }
    #endregion

    #region Collection tests

    [Test]
    public void GenerateInvoiceItemsShouldReturnCorrectItems()
    {
        // Act
        var items = cut.GenerateInvoiceItems();

        // Assert
        items.Should().NotBeEmpty();

        // Element o specyficznej nazwie
        items.Should().Contain(item => item.ProductName.Equals("laptop", StringComparison.InvariantCultureIgnoreCase));

        // czy wszystkie elementy spełniają warunek
        items.Should().OnlyContain(item => item.Quantity > 0);

        // czy elementy są prawidłowo posortowane
        items.Should().BeInAscendingOrder(item => item.Quantity); //.And.ThenBeInDescendingOrder();
    }

    #endregion

    #region Exeption tests

    [Test]
    public void GetGrossFromNetShouldThrowExceptionWhenTaxIsNegative()
    {
        cut.Invoking(invS => invS.GetGrossFromNet(10, -1))
            .Should().Throw<ArgumentException>()
            .WithMessage("Podatek nie może być ujemny*");

        Action act = () => cut.GetGrossFromNet(10, -1);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Podatek nie może być ujemny*");

        //act.Should().NotThrowAfter(10.Seconds(), 100.Microseconds()); //nie używać w testach jedn.
    }

    #endregion

    #region Event tests

    [Test]
    public void GenerateInvoiceItemsShouldRaiseInvoiceItemsGeneratedEvent()
    {
        using var monitoredSubject = cut.Monitor();
        _ = cut.GenerateInvoiceItems();
        monitoredSubject.Should().Raise(nameof(InvoiceService.InvoiceItemsGenerated));
    }

    #endregion
}