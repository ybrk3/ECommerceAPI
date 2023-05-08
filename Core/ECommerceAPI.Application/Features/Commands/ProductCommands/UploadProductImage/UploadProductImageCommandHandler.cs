using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Image;
using ECommerceAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IStorageService _storageService;
        readonly IImageFileEntityWriteRepository _imageFileEntityWriteRepository;

        public UploadProductImageCommandHandler(IProductReadRepository productReadRepository, IStorageService storageService, IImageFileEntityWriteRepository imageFileEntityWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _storageService = storageService;
            _imageFileEntityWriteRepository = imageFileEntityWriteRepository;
        }
        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            //Get the product to which photos to be uploaded
            Domain.Entities.Product? product = await _productReadRepository.GetByIdAsync(request.Id);

            //Upload photos to cloud
            List<(string fileName, string pathOrContainerName)> results = await _storageService.UploadAsync("product-images", request.Files);

            //Save to the DB
            await _imageFileEntityWriteRepository.AddRangeAsync(results.Select(r => new ImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Domain.Entities.Product> { product }
            }).ToList());
            await _imageFileEntityWriteRepository.SaveAsync();
            return new();

        }
    }
}
