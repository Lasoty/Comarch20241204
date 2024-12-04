namespace ComarchCwiczenia.Services.Model;

public class InvoiceItem
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}