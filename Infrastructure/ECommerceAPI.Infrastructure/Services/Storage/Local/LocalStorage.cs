using ECommerceAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        //to get local storage path and create folder if neccessary  wwwroot => //wwwroot/resources/product-images
        IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task Delete(string fileName, string path)
        => File.Delete($"{path}\\{fileName}");

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string fileName, string path)
        => File.Exists($"{path}\\{fileName}");



        //It's not defined in interface and private because to be used just for local storage
        async Task<bool> CopyFileAysnc(string path, IFormFile file)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream); //each file streamed
                await fileStream.FlushAsync(); //closing the file stream
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //IFormFileCollection is used to get information of files
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            //creating folders under wwwroot => //wwwroot/resources/product-images
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            //If there's no directory in the location of uploadPath, it will create
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in files)
            {
                string newFileName = await FileRenameAsync(uploadPath, file.Name, HasFile);

                await CopyFileAysnc($"{uploadPath}\\{newFileName}", file);
                datas.Add((newFileName, $"{path}\\{newFileName}"));
            }
            return datas;
        }

    }
}
