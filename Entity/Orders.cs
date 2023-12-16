using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Entity
{
    public class Orders
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public int OrderId {  get; set; }

        //constructor

        public Orders() { }
        public Orders(int orderId,int productId, int userId)
        {
            OrderId = orderId;
            ProductId = productId;
            UserId = userId;
        }
        public override string ToString()
        {
            return $"ProductId: {ProductId}, UserId: {UserId}";

        }
    }

}
