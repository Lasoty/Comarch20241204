
using ComarchCwiczenia.Services.Model;

namespace ComarchCwiczenia.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ITaxService taxService;
    private readonly IDiscountService discountService;

    public InvoiceService()
    {
        
    }

    public InvoiceService(ITaxService taxService, IDiscountService discountService)
    {
        this.taxService = taxService;
        this.discountService = discountService;
    }

    public decimal CalculateTotal(decimal amount, string customerType)
    {
        decimal discount = discountService.CalculateDiscount(amount, customerType);
        decimal taxableAmount = amount - discount;

        decimal tax = taxService.GetTax(taxableAmount);
        return taxableAmount + tax;
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
}

public interface IInvoiceService
{
    decimal CalculateTotal(decimal amount, string customerType);
}

public interface ITaxService
{
    decimal GetTax(decimal amount);
}

public interface IDiscountService
{
    decimal CalculateDiscount(decimal amount, string customerType);
}