using ECommerceAPI.Application.ViewModels.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Product.CreateProduct
{
    //class contains parameters for adding product

    public class CreateProductCommandRequest : VM_Create_Product, IRequest<CreateProductCommandResponse>
    {
    }
}
