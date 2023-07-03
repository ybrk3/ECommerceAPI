using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IProductService
    {
        //Method will create QRCode for product
        Task<byte[]> QRCodeToProductAsync(string productId);
    }
}
