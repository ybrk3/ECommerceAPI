using ECommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.GetProductImages
{
    public class GetProductImagesCommandHandler : IRequestHandler<GetProductImagesCommandRequest, List<GetProductImagesCommandResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration _configuration; 

        public GetProductImagesCommandHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<List<GetProductImagesCommandResponse>> Handle(GetProductImagesCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.ProductId));

            
            return product?.Images.Select(p => new GetProductImagesCommandResponse
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}", //Azure URL
                FileName=p.FileName,
                Id= p.Id,
                Showcase=p.Showcase,
            }).ToList();
        }
    }
}
