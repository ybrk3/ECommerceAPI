using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection service)
        {
            service.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(typeof(ServiceRegistration).Assembly); });
           
        }
    }
}
