using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Program.cs;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

class program
{
    static async Task Main()
    {
        
       

        var dbPath= GetDatabasePath();
        using (var context = new OrderDbContext(dbPath))
        {
           

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Cabinet Company Order Tracker");
                Console.WriteLine(" 1. Add orders ");
                Console.WriteLine(" 2. Search for an order ");
                Console.WriteLine(" 3. Save order to text file ");
                Console.WriteLine(" 4. Exit the Program ");
                Console.WriteLine(" Enter your choice ");
                string choice = Console.ReadLine();


                switch (choice)
                {
                    case "1":
                        await AddOrderAsync(context);
                        
                        break;
                        
                        case "2":
                            await SearchOrdersAsync(context);
                            break;
                        
                        case "3":
                            SaveOrdersToFile(context.Orders, "Orders.txt");
                            break;
                            
                        case "4":
                            Console.ForegroundColor = ConsoleColor.Red; 
                            Console.WriteLine("Exiting the program. Goodbye!");
                            
                            Environment.Exit(0); 
                            await Task.Delay(2000); // 2000 milliseconds (2 seconds)
                            Environment.Exit(0);
                            
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                          
                            
                            break;
                        
                        
                }
            }
        }
    }



    static async Task SearchOrdersAsync(OrderDbContext context)
{
    bool searchExit = false;

    while (!searchExit)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Search Orders");
        Console.WriteLine("1. Search by manufacturer name ");
        Console.WriteLine("2. Search by item Category ");
        Console.WriteLine("3. Search by PO ");
        Console.WriteLine("4. Search by Color ");
        Console.WriteLine("5. Back to main menu ");
        Console.Write("Enter your Choice: ");
        string searchChoice = Console.ReadLine();

        switch (searchChoice)
        {
            case "1":
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enter manufacturer name to search for: ");
                string manufacturerName = Console.ReadLine();

                var ordersByManufacturer = await context.Orders
                    .Where(order => order.ManufacturerName.Contains(manufacturerName))
                    .Include(order => order.Items)
                    .ToListAsync();

                DisplayOrders(ordersByManufacturer);
                break;

            case "2":
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enter item category to search for (e.g., Laminate, Hardware, Board, etc.): ");
                string itemCategory = Console.ReadLine();

                var ordersByCategory = await context.Orders
                    .Where(order => order.Items.Any(item => item.Category == itemCategory))
                    .Include(order => order.Items)
                    .ToListAsync();

                DisplayOrders(ordersByCategory);
                break;

            case "3":
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enter PO to search for: ");
                int searchPO;
                while (!int.TryParse(Console.ReadLine(), out searchPO))
                {
                    Console.WriteLine("Invalid PO. Please enter a valid integer.");
                    Console.Write("Enter PO to search for: ");
                }

                var ordersByPO = await context.Orders
                    .Where(order => order.PO == searchPO)
                    .Include(order => order.Items)
                    .ToListAsync();

                DisplayOrders(ordersByPO);
                break;

            case "4":
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enter color to search for: ");
                string searchColor = Console.ReadLine();

                var ordersByColor = await context.Orders
                    .Where(order => order.Color.Contains(searchColor))
                    .Include(order => order.Items)
                    .ToListAsync();

                DisplayOrders(ordersByColor);
                break;

            case "5":
                searchExit = true;
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
}

    
    static async Task AddOrderAsync(OrderDbContext context)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Enter the PO: ");
    int PO;
    while (!int.TryParse(Console.ReadLine(), out PO))
    {
        Console.WriteLine("Invalid PO. Please enter a valid integer.");
        Console.Write("Enter the PO: ");
    }

    Console.Write("Enter manufacturer: ");
    string manufacturerName = Console.ReadLine();

    DateTime orderDate;
    while (true)
    {
        Console.Write("Enter order date (YYYY-MM-DD): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out orderDate))
        {
            break;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date format. Please enter the date in the format YYYY-MM-DD.");
        }
    }

    Console.Write("Enter the job name: ");
    string jobName = Console.ReadLine();

    Console.Write("Enter size: ");
    string size = Console.ReadLine();

    Console.Write("Enter color: ");
    string color = Console.ReadLine();

    int quantity;
    while (true)
    {
        Console.Write("Enter quantity: ");
        if (int.TryParse(Console.ReadLine(), out quantity) && quantity > 0)
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid quantity. Please enter a positive integer.");
        }
    }

    // Create an instance of the Order entity with the provided values
    var order = new Order
    {
        PO = PO,
        JobName = jobName,
        ManufacturerName = manufacturerName,
        OrderDate = orderDate,
        Size = size,
        Color = color,
        Quantity = quantity,
        Items = new List<OrderItem>() // Initialize the Items collection
    };

    // Prompt user for OrderItem information
    Console.Write("Enter item category: ");
    string category = Console.ReadLine();

    Console.Write("Enter item description: ");
    string description = Console.ReadLine();

    // Create an instance of the OrderItem entity with the provided values
    var orderItem = new OrderItem
    {
        Category = category,
        Description = description,
        Quantity = quantity // Assuming the quantity for the item is the same as the order quantity
    };

    // Add the order item to the Items collection
    order.Items.Add(orderItem);

    // Add the order to the Orders DbSet
    context.Orders.Add(order);

    // Save changes to the database
    await context.SaveChangesAsync();

    Console.WriteLine("Order added successfully!");
    
    await Task.Delay(2000); 
}


    
    

    static void DisplayOrders(IEnumerable<Order> orders)
    {
        if (orders.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Search Results: ");
            foreach (var order in orders)
            {
                Console.WriteLine($"Order PO: {order.PO}");
                Console.WriteLine($"Job Name: {order.JobName}");
                Console.WriteLine($"Manufacturer Name: {order.ManufacturerName}");
                Console.WriteLine($"Order Date: {order.OrderDate.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Size: {order.Size}");
                Console.WriteLine($"Color: {order.Color}");
                Console.WriteLine($"Quantity: {order.Quantity}");
                Console.WriteLine("Items:");

                if (order.Items == null || !order.Items.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No order items found");
                }
                else
                {
                    foreach (var item in order.Items)
                    {
                        Console.WriteLine($"  Category: {item.Category}");
                        Console.WriteLine($"  Description: {item.Description}");
                        Console.WriteLine($"  Quantity: {item.Quantity}");
                    }
                }

                Console.WriteLine();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" No matching orders found. ");
        }
    }

    static void SaveOrdersToFile(IEnumerable<Order> orders, string filePath)
    {
        try
        {
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                foreach (var order in orders)
                {
                    writer.WriteLine($"Order PO: {order.PO}");
                    writer.WriteLine($"Manufacturer Name: {order.ManufacturerName}");
                    writer.WriteLine("Items:");
                    foreach (var item in order.Items)
                    {
                        writer.WriteLine($"  Category: {item.Category}");
                        writer.WriteLine($"  Description: {item.Description}");
                        writer.WriteLine($"  Quantity: {item.Quantity}");
                    }
                    writer.WriteLine();
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"Orders successfully saved to {filePath}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error saving orders to file: {ex.Message}");
        }
    }

    
    private static string GetDatabasePath()
    { 
        var solutionDirectory = Environment.CurrentDirectory;
        
        var dbPath = Path.Combine(Environment.CurrentDirectory, "orders.db");
        while (!solutionDirectory.EndsWith("Program.cs"))
        {
            
            
            solutionDirectory = Directory.GetParent(solutionDirectory).FullName;
        
            dbPath = Path.Combine(solutionDirectory, "orders.db");
            
            
        }
        
        Console.WriteLine(dbPath);

        return dbPath;
    }
    
    
}





