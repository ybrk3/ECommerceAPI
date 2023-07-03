using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IQRCodeService _qrCodeService;

        public ProductService(IProductReadRepository productReadRepository, IQRCodeService qrCodeService)
        {
            _productReadRepository = productReadRepository;
            _qrCodeService = qrCodeService;
        }

        public async Task<byte[]> QRCodeToProductAsync(string productId)
        {
            //Get Product
            Product product = await _productReadRepository.GetByIdAsync(productId);

            //Null Check
            if (product is null) throw new Exception("Product not found");

            //Create a object with neccessary product info
            var plainObject = new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Stock,
                product.CreatedDate,
            };
            //Serialize to JSON
            string productText = JsonSerializer.Serialize(plainObject);

            //Create and return QRCode
            return _qrCodeService.GenerateQRCode(productText);

        }
    }
}