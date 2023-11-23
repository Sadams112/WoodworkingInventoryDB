namespace Program.cs;
using Microsoft.EntityFrameworkCore;

public class Order
{
    public int PO { get; set; }
    public string JobName { get; set; }
    public List<OrderItem> Items { get; set; }
    public string ManufacturerName { get; set; }
    
    public DateTime OrderDate { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public int Quantity { get; set; }
}