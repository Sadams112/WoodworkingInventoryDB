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
                            Console.Write(" Enter item category you would like to search for (e.g., Laminate, Hardware, Board, etc: ");
                            string itemCategory = Console.ReadLine();
                            
                            //Using Linq to query Orders table
                            var ordersByCategory = context.Orders
                                .Where(order => order.Items.Any(item => item.Category == itemCategory));
                                .ToList();

                            if (ordersByCategory.Any())
                            {
                                Console.WriteLine(" Search Results: ");
                                DisplayOrders(ordersByCategory);
                            }
                            else
                            {
                                Console.WriteLine($" No orders found within the category: {itemCategory}");
                            }

                            break;
                }
                
                


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
}




