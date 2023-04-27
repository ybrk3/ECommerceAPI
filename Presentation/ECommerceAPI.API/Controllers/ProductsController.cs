using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment; //to get the path of wwwroot
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

        //There are more than 1 post functions here, that's Why we differentiate it by "[action]"
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            //creating folders under wwwroot => //wwwroot/resources/product-images
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resources/product-images");

            //If there is no directory in the location of uploadPath, below creates
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);


            Random r = new Random();
            foreach (IFormFile file in Request.Form.Files)
            {
                //creating path for each file with random number before its extension
                string fullPath = Path.Combine(uploadPath, $"{r.Next()}{Path.GetExtension(file.FileName)}");

                using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream); //each file streamed
                await fileStream.FlushAsync(); //closing the file stream
            }

            return Ok();
        }
    }
}
