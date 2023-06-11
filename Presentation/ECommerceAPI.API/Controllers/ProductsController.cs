using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerceAPI.Application.Features.Commands.Product.DeleteProduct;
using ECommerceAPI.Application.Features.Commands.ProductCommands.DeleteProductImage;
using ECommerceAPI.Application.Features.Commands.ProductCommands.GetProductImages;
using ECommerceAPI.Application.Features.Commands.ProductCommands.UpdateProduct;
using ECommerceAPI.Application.Features.Commands.ProductCommands.UploadProductImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;
using ECommerceAPI.Application.Features.Queries.GetAllProducts;
using ECommerceAPI.Application.Features.Queries.GetProductById;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Application.Repositories.Image;
using ECommerceAPI.Application.Repositories.Invoice;
using ECommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get All Products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse response = await _mediator.Send(getAllProductsQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Product By Id")]
        public async Task<IActionResult> Get([FromRoute] GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
            GetProductByIdQueryResponse products = await _mediator.Send(getProductByIdQueryRequest);
            return Ok(products);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")] //Only users who have "Admin" authorization, can use this controller
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Product")]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")] //Only users who have "Admin" authorization, can use this controller
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")] //Only users who have "Admin" authorization, can use this controller
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest deleteProductCommandRequest)
        {
            DeleteProductCommandResponse response = await _mediator.Send(deleteProductCommandRequest);
            if (response != null) return Ok();
            return BadRequest();

        }

        //There are more than 1 post functions here, that's Why we differentiate it by indicating "[action]"
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")] //Only users who have "Admin" authorization, can use this controller
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Upload Product Image")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest request)
        {
            //var datas = await _fileService.UploadAsync("resources/product-images", Request.Form.Files);
            /* await _imageFileEntityWriteRepository.AddRangeAsync(datas.Select(d => new ImageFile()
             {
                 FileName = d.fileName,
                 Path = d.path
             }).ToList());*/
            /*await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            {
                FileName = d.fileName,
                Path = d.path,
                Price = new Random().Next()
            }).ToList());*/
            /* await _fileEntityWriteRepository.AddRangeAsync(datas.Select(d => new FileEntity()
             {
                 FileName = d.fileName,
                 Path = d.path,
             }).ToList());

             await _imageFileEntityWriteRepository.SaveAsync();*/
            /*var datas = await _storageService.UploadAsync("resources/product-images", Request.Form.Files);
            await _imageFileEntityWriteRepository.AddRangeAsync(datas.Select(d => new ImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage = _storageService.StorageName
            }).ToList()); ;*/
            /* Note
             * "resources/product-images" 
             * "/" is not proper for container
             * so it needs a just name 
             */
            /*Product product = await _productReadRepository.GetByIdAsync(id);
            List<(string fileName, string pathOrContainerName)> results = await _storageService.UploadAsync("product-images", Request.Form.Files);//adds to cloud container

            await _imageFileEntityWriteRepository.AddRangeAsync(results.Select(r => new ImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product> { product }
            }).ToList());
            await _imageFileEntityWriteRepository.SaveAsync();*/
            request.Files = Request.Form.Files;
            await _mediator.Send(request);
            return Ok();
        }

        [HttpGet("[action]/{ProductId}")] //If we define it here, it means that value comes from route
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Product Images")]
        public async Task<IActionResult> getProductImages([FromRoute] GetProductImagesCommandRequest getProductImagesCommandRequest)
        {

            List<GetProductImagesCommandResponse> response = await _mediator.Send(getProductImagesCommandRequest);
            return Ok(response);
        }

        //imageId comes from query string
        [HttpDelete("[action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")] //Only users who have "Admin" authorization, can use this controller
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product Image")]
        public async Task<IActionResult> deleteImage([FromRoute] DeleteProductImageCommandRequest request, [FromQuery] string imageId)
        {
            request.ImageId = imageId;
            DeleteProductImageCommandResponse response = await _mediator.Send(request);
            return Ok();
        }


        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Change Show Case")]
        public async Task<IActionResult> ChangeShowCase([FromQuery] ChangeShowcaseImageCommandRequest request)
        {
            ChangeShowcaseImageCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

    }
}
