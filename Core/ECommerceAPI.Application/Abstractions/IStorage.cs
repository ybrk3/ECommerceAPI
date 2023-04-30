using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions
{
    public interface IStorage
    {
        //methods in common

        //IFormFileCollection is used to get information of files
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files);
        Task Delete(string fileName, string pathOrContainerName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string fileName, string pathOrContainerName);
    }
}
