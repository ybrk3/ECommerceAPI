using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Basket;
using ECommerceAPI.Application.Repositories.BasketItem;
using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Application.Repositories.Image;
using ECommerceAPI.Application.Repositories.Invoice;
using ECommerceAPI.Domain.Entities.Identity;
using ECommerceAPI.Persistence.Contexts;
using ECommerceAPI.Persistence.Repositories;
using ECommerceAPI.Persistence.Repositories.Basket;
using ECommerceAPI.Persistence.Repositories.BasketItem;
using ECommerceAPI.Persistence.Repositories.File;
using ECommerceAPI.Persistence.Repositories.Image;
using ECommerceAPI.Persistence.Repositories.Invoice;
using ECommerceAPI.Persistence.Services.Authentication;
using ECommerceAPI.Persistence.Services.Basket;
using ECommerceAPI.Persistence.Services.Order;
using ECommerceAPI.Persistence.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            //As per DB to be used, Registers the Context as a service in IServiceCollection. 
            services.AddDbContext<ECommerceAPIDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
            services.AddIdentity<AppUser, AppRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ECommerceAPIDbContext>()//If needed, password or other requirements can be altered through <AppUser,AppRole>(options=>{})
            .AddDefaultTokenProviders(); //It's added to config for Generate Password Reset Token method used in AuthService 
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IFileEntityWriteRepository, FileEntityWriteRepository>();
            services.AddScoped<IFileEntityReadRepository, FileEntityReadRepository>();
            services.AddScoped<IImageFileEntityWriteRepository, ImageFileWriteRepository>();
            services.AddScoped<IImageFileEntityReadRepository, ImageFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();
            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICompletedOrderReadRespository,CompletedOrderReadRepository>();
            services.AddScoped<ICompletedOrderWriteRespository,CompletedOrderWriteRepository>();


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IBasketService, BasketService>();
        }
    }
}
