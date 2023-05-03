using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerceAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly private BlobServiceClient _blobServiceClient; //for connection to container
        private BlobContainerClient? _blobContainerClient;
        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]); //Connecting the Azure service
        }
        public async Task Delete(string fileName, string containerName)
        {
            //Get the container in which blob to be deleted
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            //Get the file to be deleted then delete
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(d => d.Name).ToList();
        }

        public bool HasFile(string fileName, string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(d => d.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            //Gives the container name
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            //Container existence check
            await _blobContainerClient.CreateIfNotExistsAsync();
            //Accessibility of the file in the container
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            //Create a variable to return data
            List<(string fileName, string pathOrContinerName)> datas = new();

            foreach (var file in files)
            {
                string newFileName = await FileRenameAsync(containerName, file.Name, HasFile);
                BlobClient blobClient = _blobContainerClient.GetBlobClient(newFileName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((newFileName, $"{containerName}/{newFileName}"));
            }
            return datas;
        }
    }
}
