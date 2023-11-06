namespace Program.cs;

public class Order
{
    public int PO { get; set; }
    public string JobName { get; set; }
    public List<OrderItem> Items { get; set; }
    
}