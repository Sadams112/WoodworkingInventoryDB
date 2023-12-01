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
        
        var solutionDirectoryPath = GetSolutionPath();
        var dbPath = Path.Combine(solutionDirectoryPath, "orders.db");
        var filePath = Path.Combine(solutionDirectoryPath, "orders.txt");
        
        
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Woodworking order tracker ");
        Console.WriteLine("----------------------------");

        
        using (var context = new OrderDbContext(dbPath))
        {
           

            bool exit = false;

            while (!exit)
            {
                
                Console.ForegroundColor = ConsoleColor.Cyan;
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
                            SaveOrdersToFile(context.Orders, filePath);
                            break;
                            
                        case "4":
                            Console.ForegroundColor = ConsoleColor.Red; 
                            Console.WriteLine("Exiting the program. Goodbye!");
                            
                            Environment.Exit(0); 
                            await Task.Delay(2000); 
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
        string searchChoice = Console.ReadLine()?.ToLower(); 

        switch (searchChoice)
        {
            case "1":
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enter manufacturer name to search for: ");
                string manufacturerName = Console.ReadLine()?.ToLower(); 

                var ordersByManufacturer = await context.Orders
                    .Where(order => order.ManufacturerName.ToLower().Contains(manufacturerName))
                    .Include(order => order.Items)
                    .ToListAsync();

                DisplayOrders(ordersByManufacturer);
                break;

            case "2":
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enter item category to search for (e.g., Laminate, Hardware, Board, etc.): ");
                string itemCategory = Console.ReadLine()?.ToLower(); // Convert user input to lowercase

                var ordersByCategory = await context.Orders
                    .Where(order => order.Items.Any(item => item.Category.ToLower() == itemCategory))
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
                string searchColor = Console.ReadLine()?.ToLower(); 

                var ordersByColor = await context.Orders
                    .Where(order => order.Color.ToLower().Contains(searchColor))
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

    var existingOrder = await context.Orders.AnyAsync(order1 => order1.PO == PO);

    if (existingOrder)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Order already exist with PO {PO} ");
        return;
    }

    

    Console.Write("Enter manufacturer: ");
    string manufacturerName = Console.ReadLine();

    DateTime orderDate;
    while (true)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
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

    Console.ForegroundColor = ConsoleColor.Cyan;
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

    
    var order = new Order
    {
        PO = PO,
        JobName = jobName,
        ManufacturerName = manufacturerName,
        OrderDate = orderDate,
        Size = size,
        Color = color,
        Quantity = quantity,
        Items = new List<OrderItem>() 
    };

  
    Console.Write("Enter item category: ");
    string category = Console.ReadLine();

    Console.Write("Enter item description: ");
    string description = Console.ReadLine();

    
    var orderItem = new OrderItem
    {
        Category = category,
        Description = description,
        Quantity = quantity 
    };

   
    order.Items.Add(orderItem);

    
    context.Orders.Add(order);

  
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
                    writer.WriteLine($"Order Date: {order.OrderDate.ToString("yyyy-MM-dd")}");
                    writer.WriteLine($"Size: {order.Size}");
                    writer.WriteLine($"Color: {order.Color}");
                    writer.WriteLine($"Quantity: {order.Quantity}");
                    writer.WriteLine("Items:");

                    if (order.Items != null)
                    {
                        foreach (var item in order.Items)
                        {
                            writer.WriteLine($"  Category: {item.Category}");
                            writer.WriteLine($"  Description: {item.Description}");
                            writer.WriteLine($"  Quantity: {item.Quantity}");
                        }
                    }
                    else
                    {
                        writer.WriteLine("No items found");
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
        finally
        {
            Console.ResetColor(); 
        }
    }


    
    private static string GetSolutionPath()
    { 
        var solutionDirectory = Environment.CurrentDirectory;
        
       
        while (!solutionDirectory.EndsWith("Program.cs"))
        {
            
            
            solutionDirectory = Directory.GetParent(solutionDirectory).FullName;
            
            
        }
        
        Console.WriteLine(solutionDirectory);

        return solutionDirectory;
    }
    
    
}





