using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Storage
{
    //class is to be used by Client
    public class StorageService : IStorageService
    {
        //storage type (aws, local etc.) to be defined in API program.cs and also added to ServiceRegistration
        readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; } //storage type is used while file adding. No need to set it

        public async Task Delete(string fileName, string pathOrContainerName)
        => await _storage.Delete(fileName, pathOrContainerName);


        public List<string> GetFiles(string pathOrContainerName)
        => _storage.GetFiles(pathOrContainerName);

        public bool HasFile(string fileName, string pathOrContainerName)
        => _storage.HasFile(fileName, pathOrContainerName);


        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        => _storage.UploadAsync(path, files);


    }
}
