using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.SignalR
{
    public interface IProductHubService
    {
        //Method to send message when product added
        Task ProductAddedMessageAsync(string message);
    }
}
