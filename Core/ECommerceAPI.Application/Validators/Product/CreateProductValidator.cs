using ECommerceAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Validators.Product
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().WithMessage("Please do not leave product name area empty")
                .MinimumLength(2).MaximumLength(50).WithMessage("Please enter a name within 2 and 50 characters");

            RuleFor(p => p.Stock).NotEmpty().NotNull().WithMessage("Please do not leave stock number area empty")
                .Must(s => s >= 0).WithMessage("Stock number cannot be negative value");

            RuleFor(p=>p.Price).NotEmpty().NotNull().WithMessage("Please do not leave price area empty")
                .Must(p => p >= 0).WithMessage("Price cannot be negative value");
        }
    }
}
