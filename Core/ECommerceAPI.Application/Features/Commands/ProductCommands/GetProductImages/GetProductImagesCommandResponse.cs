using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.GetProductImages
{
    public class GetProductImagesCommandResponse
    {
        public string Path { get; set; }
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public bool? Showcase { get; set; }
    }
}
