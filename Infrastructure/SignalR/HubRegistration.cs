using Microsoft.AspNetCore.Builder;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalR.Hubs;

namespace SignalR
{
    public static class HubRegistration
    {
        public static void MapHubs(this Microsoft.AspNetCore.Builder.WebApplication webApplication)
        {
            //end-points to be used in UI
            webApplication.MapHub<ProductHub>("/products-hub");
            webApplication.MapHub<OrderHub>("/orders-hub");
        }
    }
}
