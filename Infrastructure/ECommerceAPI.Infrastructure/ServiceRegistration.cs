using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Infrastructure.Services;
using ECommerceAPI.Infrastructure.Services.Configurations;
using ECommerceAPI.Infrastructure.Services.Storage;
using ECommerceAPI.Infrastructure.Services.Token;
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
            service.AddScoped<ITokenHandler, TokenHandler>();
            service.AddScoped<IMailService,MailService>();
            service.AddScoped<IApplicationService, ApplicationService>();
        }

        //T must be Storage class and derived from IStorage (which are LocalStorage, AzureStorage etc.)
        public static void AddStorage<T>(this IServiceCollection service) where T : Storage, IStorage
        {
            service.AddScoped<IStorage, T>();
        }
    }
}
