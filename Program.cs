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

    class Program
    {
        static void Main(string[] args)
        {
            OrderManagementRepository Ordererpos = new OrderManagementRepository();
            Ordererpos.Run();
        }
    }
    public class OrderManagementRepository
    {

        bool isLoggedIn = false;
        int loggedInUserId;



        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                if (!isLoggedIn)
                {
                    Console.WriteLine("1. Login as Admin");
                    Console.WriteLine("2. Login as User");
                    Console.WriteLine("3. Exit");

                    Console.Write("Enter your choice: ");
                    string loginChoice = Console.ReadLine();

                    switch (loginChoice)
                    {
                        case "1":
                            AdminLogin();
                            break;
                        case "2":
                            UserLogin();
                            break;
                        case "3":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            break;
                    }
                }
                else
                {
                    // Display main menu for logged-in user
                    Console.WriteLine("1. Create User");
                    Console.WriteLine("2. Create Product");
                    Console.WriteLine("3. Create Order");
                    Console.WriteLine("4. Cancel Order");
                    Console.WriteLine("5. Get All Products");
                    Console.WriteLine("6. Get Order by User");
                    Console.WriteLine("7. Logout");

                    Console.Write("Enter your choice: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            CreateUser();
                            break;
                        case "2":
                            CreateProduct();
                            break;
                        case "3":
                            CreateOrder();
                            break;
                        case "4":
                            CancelOrder();
                            break;
                        case "5":
                            GetAllProducts();
                            break;
                        case "6":
                            GetOrderByUser();
                            break;
                        case "7":
                            Logout();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            break;
                    }
                }

                Console.WriteLine(); // Add a newline for better readability
            }
        }

        private void AdminLogin()
        {
            Console.Write("Enter Admin Username: ");
            string adminUsername = Console.ReadLine();

            Console.Write("Enter Admin Password: ");
            string adminPassword = Console.ReadLine();

            if (adminUsername == "admin" && adminPassword == "admin")
            {
                isLoggedIn = true;
                Console.WriteLine("Admin login successful!");
            }
            else
            {
                Console.WriteLine("Invalid admin credentials. Login failed.");
            }
        }

        private void UserLogin()
        {
            Console.Write("Enter User ID: ");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.Write("Enter User Password: ");
                string userPassword = Console.ReadLine();

                if (AuthenticateUser(userId, userPassword))
                {
                    isLoggedIn = true;
                    loggedInUserId = userId;
                    Console.WriteLine($"User {userId} login successful!");
                }
                else
                {
                    Console.WriteLine("Invalid user credentials. Login failed.");
                }
            }
            else
            {
                Console.WriteLine("Invalid User ID. Please enter a valid number.");
            }
        }

        private void Logout()
        {
            isLoggedIn = false;
            loggedInUserId = 0;
            Console.WriteLine("Logout successful!");
        }

        private void CreateUser()
        {
            // Implement logic for creating a user
            Console.WriteLine("CreateUser method called.");
        }

        private void CreateProduct()
        {
            // Implement logic for creating a product
            Console.WriteLine("CreateProduct method called.");
        }

        private void CreateOrder()
        {
            // Implement logic for creating an order
            Console.WriteLine("CreateOrder method called.");
        }

        private void CancelOrder()
        {
            // Implement logic for canceling an order
            Console.WriteLine("CancelOrder method called.");
        }

        private void GetAllProducts()
        {
            // Implement logic for getting all products
            Console.WriteLine("GetAllProducts method called.");
        }

        private void GetOrderByUser()
        {
            // Implement logic for getting orders by user
            Console.WriteLine("GetOrderByUser method called.");
        }

        public bool AuthenticateUser(int userId, string password)
        {
            try
            {
                string connectionString = "Server=LAPTOP-2GDF0DQD; Database=C#CodingChallengeDB; Trusted_Connection=True";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE userId = @UserId AND Password = @Password", sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@Password", password);

                        sqlConnection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            return reader.HasRows; // If there are rows, authentication is successful
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Login Failed; {ex.Message}");
                return false;
            }
        }

    }
}