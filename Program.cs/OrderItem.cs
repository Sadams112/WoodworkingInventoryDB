namespace Program.cs;
using Microsoft.EntityFrameworkCore;

public class OrderItem
{
    public int PO { get; set; }
    public string Category { get; set; } // Laminate, Hardware, Board, ETC.
    public string Description { get; set; }
    public int Quantity { get; set; }
}