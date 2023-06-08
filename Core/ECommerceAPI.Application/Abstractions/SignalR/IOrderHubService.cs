using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.SignalR
{
    public interface IOrderHubService
    {
        //Method to send message to client when order completed
        Task OrderAddedMessageAsync(string message);
    }
}
