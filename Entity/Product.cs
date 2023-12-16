using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int QuantityInStock { get; set; }
        public string Type { get; set; }
       

        // Constructors
        public Product() { }

        public Product(string productName, string description, double price, int quantityInStock, string type)
        {
          
            ProductName = productName;
            Description = description;
            Price = price;
            QuantityInStock = quantityInStock;
            Type = type;
        }

        public override string ToString() 
        {
            return $"\t ProductName = {ProductName},\t Description = {Description},\t   Price = {Price},\t QuantityInStock = {QuantityInStock} ";
        }
    }

    
}
