using System;
using System.Collections.Generic;
using System.IO;
using Program.cs;


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

                            var ordersByCategory = context.Orders
                                .Where(order => order.Items.Any(item => item.Category == itemCategory));
                }
                
                


            }
        }
    }
}




