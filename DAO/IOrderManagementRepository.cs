using Order_Management_System.Entity;
using System;
using System.Collections.Generic;
using Order_Management_System.Utility;
using System.Data.SqlClient;


namespace Order_Management_System.DAO
{
    public interface IOrderManagementRepository
    {
        int CreateOrder(User user, Product product);

            int CancelOrder(string username, int orderId);

           int CreateProduct(User user, Product product);
        int GetProductById(int productId);

            int CreateUser(User user);
            void GetAllProducts();
        List<Orders> GetOrderByUserId(int UserId);
 
        bool AuthenticateUser(string username, string password);
        int GetUserIdByUsername(string username);
        
    }
}
