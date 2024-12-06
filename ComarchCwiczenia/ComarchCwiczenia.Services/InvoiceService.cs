
using ComarchCwiczenia.Services.Model;

namespace ComarchCwiczenia.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ITaxService taxService;
    private readonly IDiscountService discountService;
    private readonly IOrderProvider orderProvider;

    public InvoiceService()
    {
        
    }

    public InvoiceService(ITaxService taxService, IDiscountService discountService, IOrderProvider orderProvider)
    {
        this.taxService = taxService;
        this.discountService = discountService;
        this.orderProvider = orderProvider;
    }

    public decimal CalculateTotal(decimal amount, string customerType)
    {
        decimal discount = discountService.CalculateDiscount(amount, customerType);
        decimal taxableAmount = amount - discount;

        decimal tax = taxService.GetTax(taxableAmount);
        return taxableAmount + tax;
    }

    public decimal CalculateInvoiceAmount(int orderId, string customerType)
    {
        decimal baseAmount = orderProvider.GetOrderAmount(orderId);

        decimal discount = discountService.CalculateDiscount(baseAmount, customerType);
        decimal amountAfterDiscount = baseAmount - discount;
        decimal tax = taxService.GetTax(amountAfterDiscount);
        return amountAfterDiscount + tax;
    }



    public event EventHandler<ICollection<InvoiceItem>>? InvoiceItemsGenerated;

    public string GenerateInvoiceNumber()
    {
        string datePart = DateTime.Now.ToString("yyyyMMdd");
        int randomPart = new Random().Next(100, 999);

        return $"INV-{datePart}-{randomPart}";
    }

    public ICollection<InvoiceItem> GenerateInvoiceItems()
    {
        ICollection<InvoiceItem> result = [
            new() { Id = Guid.NewGuid(), ProductName = "Laptop", Quantity = 1, UnitPrice = 1000m },
            new() { Id = Guid.NewGuid(), ProductName = "Smartphone", Quantity = 2, UnitPrice = 500m },
            new() { Id = Guid.NewGuid(), ProductName = "Tablet", Quantity = 3, UnitPrice = 300m }
        ];
        InvoiceItemsGenerated?.Invoke(this, result);
        return result;
    }

    public void GetGrossFromNet(decimal netValue, decimal tax)
    {
        if (tax < 0)
        {
            throw new ArgumentException("Podatek nie może być ujemny", nameof(tax));
        }
    }

    public Invoice CreateInvoice(string customerName, List<InvoiceItem> items)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Customer name cannot be empty", nameof(customerName));

        if (items == null || !items.Any())
            throw new ArgumentException("Invoice must have at least one item", nameof(items));

        decimal totalAmount = items.Sum(item => item.Quantity * item.UnitPrice);

        Invoice result = new()
        {
            Id = new Random().Next(1, 1000),
            Amount = totalAmount,
            IssueDate = DateTime.Now,
            CustomerName = customerName,
            Items = items
        };

        return result;
    }
}

public interface IInvoiceService
{
    decimal CalculateTotal(decimal amount, string customerType);
    decimal CalculateInvoiceAmount(int orderId, string customerType);
}

public interface ITaxService
{
    decimal GetTax(decimal amount);
}

public interface IDiscountService
{
    decimal CalculateDiscount(decimal amount, string customerType);
}

public interface IOrderProvider
{
    decimal GetOrderAmount(int orderId);
}