using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence
{
    //Getting Connection String from appsettings.json in /Presentation/ECommerceAPI.API
    static class Configuration
    {
       static public string ConnectionString
        {
            get {
                ConfigurationManager configurationManager = new();
                //Getting the path in which the connection string exists
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ECommerceAPI.API"));
                //Add json file at path set above
                configurationManager.AddJsonFile("appsettings.json");
                //Getting the connection string exists in "PostgreSQL"
                return configurationManager.GetConnectionString("PostgreSQL");
                }
        }
    }
}
