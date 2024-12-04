
namespace ComarchCwiczenia.Services;

public class InvoiceService
{
    public string GenerateInvoiceNumber()
    {
        string datePart = DateTime.Now.ToString("yyyyMMdd");
        int randomPart = new Random().Next(100, 999);

        return $"INV-{datePart}-{randomPart}";
    }
}