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
    public class OrderHubService : IOrderHubService
    {
        private readonly IHubContext<OrderHub> _hubContext;//no need to add it to IoC because it's already added by adding "services.AddSignalRCore();" to extended method in ServiceRegistration

        public OrderHubService(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task OrderAddedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(RecieveFunctionNames.NewOrderMessage, message);
        }
    }
}
