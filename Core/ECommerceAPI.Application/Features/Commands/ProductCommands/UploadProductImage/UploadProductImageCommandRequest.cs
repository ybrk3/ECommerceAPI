using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.UploadProductImage
{
    public class UploadProductImageCommandRequest :IRequest<UploadProductImageCommandResponse>
    {
        public string? Id { get; set; }
        public IFormFileCollection? Files { get; set; } //while first request files are null so it's nullable
    }
}
