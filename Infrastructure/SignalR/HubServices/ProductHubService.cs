using ECommerceAPI.Application.Abstractions.SignalR;
using Microsoft.AspNetCore.SignalR;
using SignalR.Constants;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {
        private readonly IHubContext<ProductHub> _hubContext; //no need to add it to IoC because it's already added by adding "services.AddSignalRCore();" to extended method in ServiceRegistration

        public ProductHubService(IHubContext<ProductHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task ProductAddedMessageAsync(string message)
        {
            //it will send given "message" to Clients.All againts "recieveProductAddedMessage" method (it's name defined in constants)
            await _hubContext.Clients.All.SendAsync(RecieveFunctionNames.ProductAddedMessage, message);

        }
    }
}
