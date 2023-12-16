using Order_Management_System.Entity;
using Order_Management_System.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
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

        public int CreateOrder(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertOrderQuery = "INSERT INTO Orders UserId ProductId)VALUES (@UserId,@ProductId); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(insertOrderQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", user.UserId);
                        cmd.Parameters.AddWithValue("@ProductId", product);
                        var rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected;
                    }
                    

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return 0;
            }

        }

        public int CancelOrder(int userId, int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Orders SET orderStatus = 'Cancelled' WHERE UserId = @UserId AND OrderId = @OrderId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@OrderId", orderId);

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


        public int CreateProduct(User adminUser, Product product)
        {
            try
            {
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();

                    
                    if (adminUser.Role != "Admin" || adminUser.Role !="admin")
                    {
                       throw new UnauthorizedAccessException("User is not authorized to create products.");
                    }

                    string query = "INSERT INTO Products (ProductName, Description, Price, QuantityInStock, Type, Brand, WarrantyPeriod, Size, Color) " +
                                   "VALUES (@ProductName, @Description, @Price, @QuantityInStock, @Type, @Brand, @WarrantyPeriod, @Size, @Color)";

                    using (SqlCommand cmd = new SqlCommand(query, sqlconnection))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                        cmd.Parameters.AddWithValue("@Description", product.Description);
                        cmd.Parameters.AddWithValue("@Price", product.Price);
                        cmd.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                        cmd.Parameters.AddWithValue("@Type", product.Type);

                        // Assuming the product is of type Electronics
                        if (product is Electronics)
                        {
                            Electronics electronicsProduct = (Electronics)product;
                            cmd.Parameters.AddWithValue("@Brand", electronicsProduct.Brand);
                            cmd.Parameters.AddWithValue("@WarrantyPeriod", electronicsProduct.WarrantyPeriod);
                        }
                        // Assuming the product is of type Clothing
                        else if (product is Clothing)
                        {
                            Clothing clothingProduct = (Clothing)product;
                            cmd.Parameters.AddWithValue("@Size", clothingProduct.Size);
                            cmd.Parameters.AddWithValue("@Color", clothingProduct.Color);
                        }

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
        public int CreateUser(User user)
        {
            try
            {
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();

                    string query = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";

                    using (SqlCommand cmd = new SqlCommand(query, sqlconnection))
                    {
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@Role", user.Role);

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
            List<Product> productList = new List<Product>();

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
                                    ProductId = (int)reader["ProductId"],
                                    ProductName = reader["ProductName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (double)reader["Price"],
                                    QuantityInStock = (int)reader["QuantityInStock"],
                                    Type = reader["Type"].ToString()
                                };
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception as needed, and consider logging or rethrowing the exception
                throw;
            }

            
        }




        public List<Product> GetOrderByUser(User user)
        {
            List<Product> orderedProducts = new List<Product>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string query = "SELECT P.* FROM Product P INNER JOIN Orders O ON P.ProductId = O.ProductId WHERE O.UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", user.UserId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
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

                                orderedProducts.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception as needed, and consider logging or rethrowing the exception
                throw;
            }

            return orderedProducts;
        }

        Orders IOrderManagementRepository.CreateOrder(User user)
        {
            throw new NotImplementedException();
        }
    }
}
