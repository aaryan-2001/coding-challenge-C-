using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
       

        // Constructors
        
        public User(string username, string password)
        {
           
            Username = username;
            Password = password;
           
        }

        public User()
        {
        }

        public override string ToString()
        {
            return $" Username: {Username}, Password: {Password}, Role: {Role}]";
        }
    }

}
