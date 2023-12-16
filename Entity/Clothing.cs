using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Entity
{
    public class Clothing : Product
    {
        public string Size { get; set; }
        public string Color { get; set; }

        // Constructors
        public Clothing() { }
        
        public Clothing( string productName, string description, double price, int quantityInStock, string type, string size, string color)
            : base( productName, description, price, quantityInStock, type)
        {
            Size = size;
            Color = color;
        }

        public override string ToString()
        {
            return $"Clothing [ ProductName: {ProductName}, Description: {Description}, Price: {Price}, QuantityInStock: {QuantityInStock}, Size: {Size}, Color: {Color}]";
        }
    }

}
