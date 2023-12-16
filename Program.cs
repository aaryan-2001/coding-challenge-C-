using Order_Management_System.DAO;
using Order_Management_System.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System
{
    namespace Order_Management_System
{
        internal class Program
        {
            static void Main(string[] args)
            {
                IOrderManagementRepository Ordererpos = new OrderManagementRepository();
                bool isLoggedIn = false;

                #region
                while (true)
                {

                    Console.WriteLine("==============================================================");
                    Console.WriteLine("                   Order Management					");
                    Console.WriteLine("==============================================================\n");

                    Console.WriteLine("1. Login");
                  
                    Console.WriteLine("2. Exit");

                    Console.Write("\nPlease Enter your choice: ");
                    string choice = Console.ReadLine();
                    Console.WriteLine();

                    switch (choice)
                    {
                        case "1":
                            if (!isLoggedIn)
                            {
                                Console.WriteLine("==============================================================");
                                Console.WriteLine("                       LOGIN WINDOW					");
                                Console.WriteLine("==============================================================\n");
                                Console.Write("Enter username: ");
                                string username = Console.ReadLine();
                                Console.Write("Enter password: ");
                                string password = Console.ReadLine();



                                if (Ordererpos.AuthenticateUser(username, password))
                                {
                                    Console.WriteLine("\nLogin successful!\n");
                                    isLoggedIn = true;
                                    string loggedInUsername = username;

                                    while (true)
                                    {
                                        Console.WriteLine($"\nWelcome, {loggedInUsername}!\n");
                                        Console.WriteLine("1. Create User");
                                        Console.WriteLine("2. Create Product");
                                        Console.WriteLine("3. Create Order");
                                        Console.WriteLine("4. Cancel Order");
                                        Console.WriteLine("5. Get All Products");
                                        Console.WriteLine("6. Get Order by User");
                                        Console.WriteLine("7. Logout");


                                        Console.Write("\nPlease Enter your choice: ");
                                        string userProfileChoice = Console.ReadLine();

                                        switch (userProfileChoice)
                                        {
                                            case "1":

                                                Console.WriteLine("==============================================================");
                                                Console.WriteLine("                       UPDATE PROFILE WINDOW					");
                                                Console.WriteLine("==============================================================\n");

                                                if (isLoggedIn)
                                                {
                                                    Console.Write("Enter new UserName: ");
                                                    string new_username = Console.ReadLine();
                                                    Console.Write("Enter new password: ");
                                                    string new_Password = Console.ReadLine();
                                                    int success = Ordererpos.CreateUser(new User()

                                                    {
                                                        Username = new_username,
                                                        Password = new_Password,

                                                    });

                                                }
                                                break;
                                            case "2":
                                                if (isLoggedIn && loggedInUsername == "admin")
                                                {
                                                    User user = new User
                                                    {
                                                        Username = "admin",
                                                        Password = "admin"
                                                    };

                                                    Console.Write("Enter Product Name: ");
                                                    string productName = Console.ReadLine();

                                                    Console.Write("Description: ");
                                                    string description = Console.ReadLine();

                                                    Console.Write("Price: ");
                                                    double price = double.Parse(Console.ReadLine());

                                                    Console.Write("Quantity in Stock: ");
                                                    int quantityInStock = int.Parse(Console.ReadLine());

                                                    Console.Write("Enter Product Type (Electronics/Clothing): ");
                                                    string productType = Console.ReadLine();


                                                    Product product;


                                                    if (productType.ToLower() == "electronics")
                                                    {
                                                        Console.Write("Enter Electronics Brand: ");
                                                        string brand = Console.ReadLine();

                                                        Console.Write("Enter Electronics Warranty Period: ");
                                                        int warrantyPeriod = int.Parse(Console.ReadLine());

                                                        product = new Electronics(productName, description, price, quantityInStock, productType, brand, warrantyPeriod);
                                                    }
                                                    else if (productType.ToLower() == "clothing")
                                                    {
                                                        Console.Write("Enter Clothing Size: ");
                                                        string size = Console.ReadLine();

                                                        Console.Write("Enter Clothing Color: ");
                                                        string color = Console.ReadLine();

                                                        product = new Clothing(productName, description, price, quantityInStock, productType, size, color);
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Invalid product type. Product not added.");
                                                        return;
                                                    }
                                                    Ordererpos.CreateProduct(user, product);

                                                }

                                                break;
                                            case "3":
                                                if (isLoggedIn)
                                                {
                                                    User user = new User
                                                    {
                                                        UserId =  Ordererpos.GetUserIdByUsername(loggedInUsername)
                                                         
                                                };
                                                    

                                                    Console.Write("Enter Product ID: ");
                                                    if (int.TryParse(Console.ReadLine(), out int productIds))
                                                    {
                                                        Product product = new Product
                                                        {
                                                            ProductId = Ordererpos.GetProductById(productIds)
                                                        };
                                                        if (productIds !=0)
                                                        {
                                                           int orders= Ordererpos.CreateOrder(user, product);

                                                            if (orders > 0)
                                                            {
                                                                Console.WriteLine($"Order created successfully. OrderId: {orders}");
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Failed to create the order.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Product not found.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Invalid Product ID. Please enter a valid number.");
                                                    }
                                                    break;
                                                }

                                                break;
                                            case "4":

                                                Console.Write("Enter Order ID to cancel: ");
                                                if (int.TryParse(Console.ReadLine(), out int orderId))
                                                {
                                                    var result = Ordererpos.CancelOrder(loggedInUsername, orderId);

                                                    if (result > 0)
                                                    {
                                                        Console.WriteLine("Order cancelled successfully!");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Failed to cancel order. Please check the Order ID and try again.");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Invalid input for Order ID. Please enter a valid number.");
                                                }
                                                break;
                                            case "5":
                                                if (isLoggedIn)
                                                {
                                                    Ordererpos.GetAllProducts();
                                                }
                                                break;

                                            case "6":
                                                if (isLoggedIn)
                                                {
                                                    User user = new User
                                                    {
                                                        //Username = loggedInUsername,
                                                        UserId = Ordererpos.GetUserIdByUsername(loggedInUsername)

                                                    };

                                                    Ordererpos.GetOrderByUserId(user.UserId);

                                                   
                                                }
                                                break;

                                            case "7":
                                                if (isLoggedIn)
                                                {
                                                    Console.WriteLine("Logging You Out.....");

                                                    isLoggedIn = false; // Set to false after logout
                                                    Console.Clear();

                                                }
                                                break;

                                            default:
                                                Console.WriteLine("Invalid choice. Please enter a valid option.");
                                                break;
                                        }

                                        if (userProfileChoice == "7")
                                            break; // Return to the main menu
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid username or password. Login failed.");
                                }
                            }
                            break;






                        case "2":
                            Console.WriteLine("Exiting 'Thank You'......");
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            break;
                    }

                    Console.WriteLine();
                }



            }
        
        }
    }
}

#endregion