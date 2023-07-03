using ECommerceAPI.Application.Abstractions.Services;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.QRCode
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQRCode(string text)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new PngByteQRCode(qRCodeData);
            byte[] byteGraphic = qRCode.GetGraphic(10, new byte[] { 84, 99, 71 }, new byte[] { 240, 240, 240 });
            return byteGraphic;

        }
    }
}
