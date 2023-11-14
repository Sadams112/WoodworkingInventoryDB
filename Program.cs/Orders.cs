namespace Program.cs;
using Microsoft.EntityFrameworkCore;

public class Order
{
    public int PO { get; set; }
    public string JobName { get; set; }
    public List<OrderItem> Items { get; set; }
    public string ManufacturerName { get; set; }
}