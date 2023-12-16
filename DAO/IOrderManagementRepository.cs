using Order_Management_System.Entity;
using System;
using System.Collections.Generic;
using Order_Management_System.Utility;


namespace Order_Management_System.DAO
{
    public interface IOrderManagementRepository
    {
          Orders CreateOrder(User user);

            int CancelOrder(int userId, int orderId);

           int CreateProduct(User adminUser, Product product);


            int CreateUser(User user);
            void GetAllProducts();

            List<Product> GetOrderByUser(User user);
        
    }
}
