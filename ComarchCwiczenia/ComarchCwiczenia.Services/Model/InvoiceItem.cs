namespace ComarchCwiczenia.Services.Model;

public class InvoiceItem
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

}

public class Invoice
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime IssueDate { get; set; }
    public string CustomerName { get; set; }
    public List<InvoiceItem> Items { get; set; }
    public string CustomerEmail { get; set; }
}