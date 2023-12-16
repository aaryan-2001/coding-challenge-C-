using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Order_Management_System.Utility
{
    public class DbConnUtil
    {
        private static IConfiguration _iconfiguration;
        static DbConnUtil()
        {
            GetAppSettingsFile();
        }

        private static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath("E:/VISUAL STUDIO ASPNET/Repository/Order Management System/Order Management System")
                        .AddJsonFile("Appsettings.json");
            _iconfiguration = builder.Build();

        }
        public static string GetConnectionString()
        {
            return _iconfiguration.GetConnectionString("LocalConnectionString");
        }
    }
}
