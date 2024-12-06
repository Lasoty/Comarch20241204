using AutoFixture;
using ComarchCwiczenia.Services;
using ComarchCwiczenia.Services.Model;
using FluentAssertions;

namespace ComarchCwiczenia.Unit.Test.Services;

public class InvoiceServiceAutofixtureTests
{
    private Fixture fixture;
    private InvoiceService invoiceService;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture();
        invoiceService = new InvoiceService();
    }

    [Test]
    public void CreateInvoiceShouldCreateWithItemsSuccessfully()
    {
        // Arrange
        string customerName = fixture.Create<string>();
        List<InvoiceItem> items = fixture.CreateMany<InvoiceItem>(3).ToList();

        // Act
        Invoice result = invoiceService.CreateInvoice(customerName, items);

        // Assert
        result.Should().NotBeNull();
        result.CustomerName.Should().Be(customerName);
        result.Items.Should().NotBeEmpty().And.HaveCount(3);
        result.Amount.Should().Be(items.Sum(i => i.Quantity * i.UnitPrice));
    }

    [Test]
    public void ShouldCreateInvoiceWithSpecificItemPrice()
    {
        fixture.Customize<InvoiceItem>(ii => ii.With(x => x.UnitPrice, 100m));
        string customerName = fixture.Create<string>();
        List<InvoiceItem> items = fixture.CreateMany<InvoiceItem>(3).ToList();

        // Act
        Invoice result = invoiceService.CreateInvoice(customerName, items);

        // Assert
        result.Items.All(ii => ii.UnitPrice == 100m).Should().BeTrue();
    }
}