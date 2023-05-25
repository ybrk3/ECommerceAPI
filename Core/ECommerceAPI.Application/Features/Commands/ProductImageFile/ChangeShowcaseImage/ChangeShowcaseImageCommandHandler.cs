using ECommerceAPI.Application.Repositories.Image;
using Google.Apis.Util;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
    {
        private readonly IImageFileEntityWriteRepository _imageFileEntityWriteRepository;

        public ChangeShowcaseImageCommandHandler(IImageFileEntityWriteRepository imageFileEntityWriteRepository)
        {
            _imageFileEntityWriteRepository = imageFileEntityWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            //many-to-many relationship
            //we selected images with product
            var query = _imageFileEntityWriteRepository.Table
                                .Include(p => p.Products)
                                .SelectMany(p => p.Products, (pif, p) => new
                                {
                                    p,
                                    pif,
                                });

            //Get the image in which showcase is true
            var data = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(request.ProductId) && p.pif.Showcase);

            //Change showcase value as false
            if (data != null) data.pif.Showcase = false;

            //get selected photo
            var image = await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(request.ImageId));
            //and change its showcase property
           if(image!=null) image.pif.Showcase = true;

            await _imageFileEntityWriteRepository.SaveAsync();

            return new();
            
            
            throw new NotImplementedException();
        }
    }
}
