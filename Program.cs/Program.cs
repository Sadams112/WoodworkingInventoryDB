using System;
using System.Collections.Generic;
using System.IO;
using Program.cs;
using System.Linq;

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
                        Console.WriteLine(" What is the category? ");
                        string category = Console.ReadLine();
                        
                        Console.WriteLine(" What is the PO ");
                        int PO = Console.Read();

                        Console.WriteLine(" What is the order date? ");
                        string date = Console.ReadLine();
                        
                        Console.WriteLine(" What is the Size? ");
                        string size = Console.ReadLine();
                        
                        Console.WriteLine(" What is the Color? ");
                        string Color = Console.ReadLine();
                        
                        Console.WriteLine(" Who is the Manufacturer? ");
                        string Manufacturer = Console.ReadLine():
                        
                        Console.WriteLine(" What is the Quantity? ");
                        string Quantity = Console.ReadLine();
                        
                        break;
                        
                        case "2":
                            SearchOrders(context);
                            break;
                            
                }
            }
        }
    }



    static void SearchOrders(OrderDbConext context)
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
                    writer.WriteLine($"Manufacturer Name: {order.manufacturerName}");
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





