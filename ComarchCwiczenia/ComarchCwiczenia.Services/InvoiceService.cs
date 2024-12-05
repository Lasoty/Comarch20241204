
using ComarchCwiczenia.Services.Model;

namespace ComarchCwiczenia.Services;

public class InvoiceService
{
    public string GenerateInvoiceNumber()
    {
        string datePart = DateTime.Now.ToString("yyyyMMdd");
        int randomPart = new Random().Next(100, 999);

        return $"INV-{datePart}-{randomPart}";
    }

    public ICollection<InvoiceItem> GenerateInvoiceItems()
    {
        return [
            new() { Id = Guid.NewGuid(), ProductName = "Laptop", Quantity = 1, UnitPrice = 1000m },
            new() { Id = Guid.NewGuid(), ProductName = "Smartphone", Quantity = 2, UnitPrice = 500m },
            new() { Id = Guid.NewGuid(), ProductName = "Tablet", Quantity = 3, UnitPrice = 300m }
        ];
    }

    public void GetGrossFromNet(decimal netValue, decimal tax)
    {
        if (tax < 0)
        {
            throw new ArgumentException("Podatek nie może być ujemny", nameof(tax));
        }
    }
}