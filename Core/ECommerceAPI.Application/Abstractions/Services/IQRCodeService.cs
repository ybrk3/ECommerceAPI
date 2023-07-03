using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IQRCodeService
    {
        //this method to be used in product service
        byte[] GenerateQRCode(string text);
    }
}
