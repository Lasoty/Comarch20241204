using ComarchCwiczenia.Services;
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
}