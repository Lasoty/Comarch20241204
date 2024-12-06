using System.Security.Cryptography.X509Certificates;
using ComarchCwiczenia.Services;
using ComarchCwiczenia.Services.Model;
using NUnit.Framework.Legacy;

namespace ComarchCwiczenia.Unit.Test.Services;

[TestFixture]
public class InvoiceServiceTests
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
    #endregion

    #region Collection Tests

    [Test]
    public void GenerateInvoiceItemsShouldReturnNotEmptyCollection()
    {
        // Act
        ICollection<InvoiceItem> items = cut.GenerateInvoiceItems();

        // Assert
        CollectionAssert.IsNotEmpty(items);
        Assert.That(items, Does.Not.Empty);
    }

    [Test]
    public void GenerateInvoiceItemsShouldReturnCorrectCollection()
    {
        // Act
        ICollection<InvoiceItem> items = cut.GenerateInvoiceItems();

        // Assert
        Assert.That(items, Does.Not.Empty.And.Contain(items.First(x => x.ProductName.Equals("Laptop"))));
        Assert.That(items.Any(x => x.Quantity <= 0), Is.False);
        Assert.That(items.Count, Is.GreaterThan(1).And.LessThan(4));
    }

    #endregion

    #region Exception tests

    [Test]
    public void GetGrossFromNetShouldThrowsExceptionWhenTaxIsNegative()
    {
        // Arrange
        decimal netValue = 10m;
        decimal tax = -1m;

        //Act & Assert
        Assert.Throws<ArgumentException>(() => cut.GetGrossFromNet(netValue, tax));
    }

    #endregion
}