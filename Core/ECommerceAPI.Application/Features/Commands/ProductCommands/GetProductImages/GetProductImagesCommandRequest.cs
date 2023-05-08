using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.GetProductImages
{
    public class GetProductImagesCommandRequest : IRequest<List<GetProductImagesCommandResponse>>
    {
        public string ProductId { get; set; }
    }
}
