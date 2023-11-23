using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Program.cs;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

class program
{
    static void Main()
    {
        
        
        using (var context = new OrderDbContext())
        {
            context.Database.EnsureCreated();

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Cabinet Company Order Tracker");
                Console.WriteLine(" .1 Add orders ");
                Console.WriteLine(" 2. Search for an order ");
                Console.WriteLine(" 3. Save order to text file ");
                Console.WriteLine(" 4. Exit the Program ");
                Console.WriteLine(" Enter your choice ");
                string choice = Console.ReadLine();


                switch (choice)
                {
                    case "1":
                        AddOrder(context);
                        
                        break;
                        
                        case "2":
                            SearchOrders(context);
                            break;
                        
                        case "3":
                            SaveOrdersToFile(context.Orders, "Orders.txt");
                            break;
                            
                        case "4":
                             
                            Console.WriteLine("Exiting the program. Goodbye!");
                            
                            Environment.Exit(0); // Terminate the program with exit code 0
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                            break;
                        
                        
                }
            }
        }
    }



    static void SearchOrders(OrderDbContext context)
    {
        bool searchExit = false;

        while (!searchExit)
        {
            Console.WriteLine("Search Orders");
            Console.WriteLine("1. Search by manufacturer name ");
            Console.WriteLine("2. Search by item Category ");
            Console.WriteLine("3. Back to main menu ");
            Console.Write("Enter your Choice: ");
            string searchChoice = Console.ReadLine();


            switch (searchChoice)
            {
                case "1":
                    Console.Write("Enter manufacturer name to search for: ");
                    string manufacturerName = Console.ReadLine();

                    var ordersByManufacturer = context.Orders
                        .Where(order => order.ManufacturerName.Contains(manufacturerName))
                        .ToList();  // Using ToList to materialize the results

                    DisplayOrders(ordersByManufacturer);
                    break;
                
                case "2":
                    Console.Write("Enter item category to search for (e.g., Laminate, Hardware, Board, etc.): ");
                    string itemCategory = Console.ReadLine();

                    var ordersByCategory = context.Orders
                        .Where(order => order.Items.Any(item => item.Category == itemCategory))
                        .ToList();  // Using ToList to materialize the results

                    DisplayOrders(ordersByCategory);
                    break;

                case "3":
                    searchExit = true;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

    }

    
    static void AddOrder(OrderDbContext context)
    {
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
                Console.WriteLine("Invalid date format. Please enter the date in the format YYYY-MM-DD.");
            }
        }

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
            ManufacturerName = manufacturerName,
            OrderDate = orderDate,
            Size = size,
            Color = color,
            Quantity = quantity
        };

        // Add the order to the Orders DbSet
        context.Orders.Add(order);

        // Save changes to the database
        context.SaveChanges();

        Console.WriteLine("Order added successfully!");
    }

    
    

    static void DisplayOrders(IEnumerable<Order> orders)
    {
        if (orders.Any())
        {
            Console.WriteLine(" Search Results: ");
            foreach (var order in orders)
            {
                Console.WriteLine($"OrderPO: {order.PO}");
                Console.WriteLine($"Job Name: {order.JobName}");
                Console.WriteLine($"Items:");

                foreach (var item in order.Items)
                    
                {
                    Console.WriteLine($" Category: {item.Category}");
                    Console.WriteLine($" Description: {item.Description}");
                    Console.WriteLine($" Quantity: {item.Quantity}");
                    
                }
                Console.WriteLine();
            }
        }
        else
        {
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

            Console.WriteLine($"Orders successfully saved to {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving orders to file: {ex.Message}");
        }
    }

}





