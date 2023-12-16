using Order_Management_System.Entity;
using Order_Management_System.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.DAO
{
    public class OrderManagementRepository : IOrderManagementRepository
    {


        List<Product> product = new List<Product>();
        public string connectionString;
        SqlCommand cmd = null;

        

        //constructor

        public OrderManagementRepository()
        {
            connectionString = DbConnUtil.GetConnectionString();
            cmd = new SqlCommand();
        }

        public int CreateOrder(User user, Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertOrderQuery = "INSERT INTO Orders (UserId, ProductId) VALUES (@UserId, @ProductId); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(insertOrderQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", user.UserId);
                        cmd.Parameters.AddWithValue("@ProductId", product.ProductId);

                        var orderId = Convert.ToInt32(cmd.ExecuteScalar());
                       
                        bool success = int.TryParse(orderId.ToString(), out orderId);

                        if (success)
                        {
                            return orderId;
                        }
                        else
                        {
                            Console.WriteLine($"Error converting order id: {orderId}");
                            return 0;
                        }

                       /// return orderId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                return 0;
            }
        }

        public int CancelOrder(string username, int orderId)
        {
           
            try
            {
                int userId = GetUserIdByUsername(username);

                if (userId > 0)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string cancelOrderQuery = "DELETE FROM Orders WHERE UserId = @UserId AND OrderId = @OrderId";

                        using (SqlCommand cmd = new SqlCommand(cancelOrderQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.Parameters.AddWithValue("@OrderId", orderId);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            return rowsAffected;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"User with username '{username}' not found. Cannot cancel order.");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

        }

        public int CreateProduct(User user, Product product)
            {
            try
            {
                if (user.Username == "admin" && user.Password == "admin")
                {

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query1 = "INSERT INTO Product (ProductName, Description, Price, QuantityInStock, Type, Brand, WarrantyPeriod) VALUES (@ProductName, @Description, @Price, @QuantityInStock, @Type, @Brand, @WarrantyPeriod,)";
                        string query = "INSERT INTO Product (ProductName, Description, Price, QuantityInStock, Type,Size, Color) VALUES (@ProductName, @Description, @Price, @QuantityInStock, @Type,  @Size, @Color)";

                        using (SqlCommand cmd = new SqlCommand(query1, connection))
                        {
                            if (product is Electronics electronics)
                            {
                                cmd.Parameters.AddWithValue("@Brand", electronics.Brand);
                                cmd.Parameters.AddWithValue("@WarrantyPeriod", electronics.WarrantyPeriod);
                                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                                cmd.Parameters.AddWithValue("@Description", product.Description);
                                cmd.Parameters.AddWithValue("@Price", product.Price);
                                cmd.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                                cmd.Parameters.AddWithValue("@Type", product.Type);
                            }
                        }
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            // Add parameters specific to electronics or clothing

                            if (product is Clothing clothing)
                            {
                                cmd.Parameters.AddWithValue("@Size", clothing.Size);
                                cmd.Parameters.AddWithValue("@Color", clothing.Color);
                                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                                cmd.Parameters.AddWithValue("@Description", product.Description);
                                cmd.Parameters.AddWithValue("@Price", product.Price);
                                cmd.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                                cmd.Parameters.AddWithValue("@Type", product.Type);
                            }

                            int rowsAffected = cmd.ExecuteNonQuery();

                            return rowsAffected;

                        }

                    }
                    
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
                return 0;
            }
            }
        
        public int CreateUser(User user)
        {
            try
            {
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();

                    string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

                    using (SqlCommand cmd = new SqlCommand(query, sqlconnection))
                    {
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                       

                        var rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public void GetAllProducts()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Product";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = Convert.ToDouble(reader["Price"]), // Use Convert.ToDouble
                                    QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]), // Use Convert.ToInt32
                                    Type = reader["Type"].ToString()
                                };
                                Console.WriteLine($" ProductName = {product.ProductName},\t Description = {product.Description},\t Price = {product.Price},\t QuantityInStock = {product.QuantityInStock} ");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<Orders>  GetOrderByUserId(int userId)
        {
            List<Orders> orders = new List<Orders>();

            try
            {
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();

                    string query = "SELECT * FROM Orders WHERE UserId = @UserId";

                    using (SqlCommand command = new SqlCommand(query, sqlconnection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int order_Id = Convert.ToInt32(reader["OrderId"]);
                                int user_Id = Convert.ToInt32(reader["UserId"]);
                                int product_Id = Convert.ToInt32(reader["ProductId"]);

                                Orders order = new Orders
                                {
                                    OrderId = order_Id,
                                    UserId = user_Id,
                                    ProductId = product_Id
                                };

                                orders.Add(order);
                                
                            }
                            foreach (var item in orders)
                            {
                                Console.WriteLine(item);

                            }
                        }
                    }
                }
                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting orders: {ex.Message}");
            }
            
            return orders;
            
            
        }

        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                string connectionString = "Server=LAPTOP-2GDF0DQD; Database=C#CodingChallengeDB; Trusted_Connection=True";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE username = @Username AND Password = @Password", sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
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



        public int GetUserIdByUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string getUserIdQuery = "SELECT UserId FROM Users WHERE Username = @Username";

                using (SqlCommand cmd = new SqlCommand(getUserIdQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    object result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public int GetProductById(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Product WHERE ProductId = @ProductId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Product product = new Product
                                {
                                    ProductId = (int)reader["ProductId"],
                                    ProductName = reader["ProductName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (double)reader["Price"],
                                    QuantityInStock = (int)reader["QuantityInStock"],
                                    Type = reader["Type"].ToString()
                                };

                                // Check if the product is Electronics or Clothing and set additional properties accordingly

                            }
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product: {ex.Message}");
                return 0;
            }
        }


    }
}
