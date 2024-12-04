using ComarchCwiczenia.Services;

namespace ComarchCwiczenia.Unit.Test.Services;

[TestFixture]
public class InvoiceServiceTests
{
    private InvoiceService cut;

    [SetUp]
    public void Setup()
    {
        cut = new InvoiceService();
    }

    [Test]
    public void GenerateInvoiceNumberShouldStartWithINV()
    {
        // Act
        string actual = cut.GenerateInvoiceNumber();

        // Assert
        Assert.That(actual, Does.StartWith("INV-"));
    }

    [Test]
    public void GenerateInvoiceNumberShouldEndWithNumericSuffix()
    {
        // Act
        string actual = cut.GenerateInvoiceNumber();

        // Assert
        Assert.That(actual, Does.Match(@"INV-\d{8}-\d{3}$"));
    }

    [Test]
    public void GenerateInvoiceNumberShouldContainCurrentDate()
    {
        // Arrange
        string currentDate = DateTime.Now.ToString("yyyyMMdd");

        // Act
        string actual = cut.GenerateInvoiceNumber();

        // Assert
        Assert.That(actual, Does.Contain(currentDate));
    }

    [Test]
    public void GenerateInvoiceNumberShouldHaveLengthOf16()
    {
        // Act
        string actual = cut.GenerateInvoiceNumber();

        // Assert
        Assert.That(actual, Is.Not.Empty);
        Assert.That(actual.Length, Is.EqualTo(16));
    }
}