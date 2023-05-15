using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerceAPI.Application.Features.Commands.Product.DeleteProduct;
using ECommerceAPI.Application.Features.Commands.ProductCommands.DeleteProductImage;
using ECommerceAPI.Application.Features.Commands.ProductCommands.GetProductImages;
using ECommerceAPI.Application.Features.Commands.ProductCommands.UpdateProduct;
using ECommerceAPI.Application.Features.Commands.ProductCommands.UploadProductImage;
using ECommerceAPI.Application.Features.Queries.GetAllProducts;
using ECommerceAPI.Application.Features.Queries.GetProductById;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Application.Repositories.Image;
using ECommerceAPI.Application.Repositories.Invoice;
using ECommerceAPI.Domain.Entities;
using MediatR;
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
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IWebHostEnvironment _webHostEnvironment;
        readonly private IImageFileEntityWriteRepository _imageFileEntityWriteRepository;
        readonly private IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly private IFileEntityWriteRepository _fileEntityWriteRepository;
        readonly private IConfiguration _configuration;
        IStorageService _storageService;
        readonly private IImageFileEntityReadRepository _imageFileEntityReadRepository;
        private IMediator _mediator;



        public ProductsController(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IWebHostEnvironment webHostEnvironment,
            IImageFileEntityWriteRepository imageFileEntityWriteRepository,
            IConfiguration configuration,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IFileEntityWriteRepository fileEntityWriteRepository,
            IStorageService storageService,
            IImageFileEntityReadRepository imageFileEntityReadRepository,
            IMediator mediator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment; //to get the path of wwwroot
            _imageFileEntityWriteRepository = imageFileEntityWriteRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _fileEntityWriteRepository = fileEntityWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _imageFileEntityReadRepository = imageFileEntityReadRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse response = await _mediator.Send(getAllProductsQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
            GetProductByIdQueryResponse products = await _mediator.Send(getProductByIdQueryRequest);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")] 
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest deleteProductCommandRequest)
        {
            DeleteProductCommandResponse response = await _mediator.Send(deleteProductCommandRequest);
            if (response != null) return Ok();
            return BadRequest();

        }

        //There are more than 1 post functions here, that's Why we differentiate it by indicating "[action]"
        [HttpPost("[action]")]
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

        [HttpGet("[action]/{ProductId}")]
        public async Task<IActionResult> getProductImages([FromRoute] GetProductImagesCommandRequest getProductImagesCommandRequest)
        {

            List<GetProductImagesCommandResponse>  response=await _mediator.Send(getProductImagesCommandRequest);
            return Ok(response);
        }

        //imageId comes from query string
        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> deleteImage([FromRoute] DeleteProductImageCommandRequest request, [FromQuery] string imageId)
        {
            request.ImageId = imageId;
            DeleteProductImageCommandResponse response = await _mediator.Send(request);
            return Ok();
        }
    }
}
