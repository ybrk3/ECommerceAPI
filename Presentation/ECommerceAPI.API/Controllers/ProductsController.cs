using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Application.Repositories.Image;
using ECommerceAPI.Application.Repositories.Invoice;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Services;
using ECommerceAPI.Persistence.Repositories.Image;
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
        public ProductsController(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IWebHostEnvironment webHostEnvironment,
            IImageFileEntityWriteRepository imageFileEntityWriteRepository,
            IConfiguration configuration,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IFileEntityWriteRepository fileEntityWriteRepository,
            IStorageService storageService,
            IImageFileEntityReadRepository imageFileEntityReadRepository)
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
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] Pagination pagination)

        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            { p.Id, p.Name, p.Stock, p.Price, p.CreatedDate, p.UpdatedDate });

            return Ok(new { totalCount, products });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {

            return Ok(_productReadRepository.GetByIdAsync(id, false));
        }


        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            await _productWriteRepository.AddAsync(new Product
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price,
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product productToUpdate = await _productReadRepository.GetByIdAsync(model.Id);
            productToUpdate.Name = model.Name;
            productToUpdate.Stock = model.Stock;
            productToUpdate.Price = model.Price;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.Remove(id);
            await _productWriteRepository.SaveAsync();
            return Ok();

        }

        //There are more than 1 post functions here, that's Why we differentiate it by indicating "[action]"
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
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
            Product product = await _productReadRepository.GetByIdAsync(id);
            List<(string fileName, string pathOrContainerName)> results = await _storageService.UploadAsync("product-images", Request.Form.Files);//adds to cloud container

            await _imageFileEntityWriteRepository.AddRangeAsync(results.Select(r => new ImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product> { product }
            }).ToList());
            await _imageFileEntityWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> getProductImages(string productId)
        {

            Product? product = await _productReadRepository.Table.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));

            return Ok(product?.Images.Select(i => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{i.Path}",
                i.FileName,
                i.Id
            }));
        }

        //imageId comes from query string
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> deleteImage(string id, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            ImageFile? imageToDelete = product?.Images.FirstOrDefault(i => i.Id == Guid.Parse(imageId));

            if (imageToDelete != null) product?.Images.Remove(imageToDelete);

            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
