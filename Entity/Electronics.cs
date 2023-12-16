using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Entity
{
    public class Electronics : Product
    {
        public string Brand { get; set; }
        public int WarrantyPeriod { get; set; }

        // Constructors
       
        public Electronics( string productName, string description, double price, int quantityInStock, string type, string brand, int warrantyPeriod)
            : base( productName, description, price, quantityInStock, type)
        {
            Brand = brand;
            WarrantyPeriod = warrantyPeriod;
        }

        public override string ToString()
        {
            return $"Electronics [ ProductName: {ProductName}, Description: {Description}, Price: {Price}, QuantityInStock: {QuantityInStock}, Brand: {Brand}, WarrantyPeriod: {WarrantyPeriod}]";
        }
    }

}
