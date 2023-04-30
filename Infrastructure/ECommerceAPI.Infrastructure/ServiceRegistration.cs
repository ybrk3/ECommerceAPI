using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Infrastructure.Services;
using ECommerceAPI.Infrastructure.Services.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddScoped<IStorageService, StorageService>();
        }

        //T must be class derived from IStorage (which are LocalStorage, AzureStorage etc.)
        public static void AddStorage<T>(this IServiceCollection service) where T : class, IStorage
        {
            service.AddScoped<IStorage, T>();
        }
    }
}
