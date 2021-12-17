using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SimpleQRCodeGenerator.Controllers
{
    [ApiController]
    public class QRCodeImageGeneratorController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public QRCodeImageGeneratorController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }



        [HttpPost]
        [Route("api/QRCodeImageGenerator/QRCode")]
        public IActionResult QRCodeImageGenerator(QRCodeModel data)
        {
            QRCodeGenerator qr = new QRCodeGenerator();

            foreach (var qrcode in data.ListText)
            {
                QRCodeData dataw = qr.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(dataw);
                Bitmap bitmap = qRCode.GetGraphic(5);
                byte[] arrayOfBytes = null;

                arrayOfBytes = ImageToArrayOfBytes(bitmap);

                SaveQrcode(Guid.NewGuid().ToString(), arrayOfBytes);

            }
            return Ok("Generated :)");

        }


        private byte[] ImageToArrayOfBytes(System.Drawing.Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }



        private void SaveQrcode(string ImageName, byte[] byteImage)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot");

            string fullimageName = ImageName + ".png";

            string filePath = Path.Combine(uploadsFolder, fullimageName);

            System.IO.File.WriteAllBytes(filePath, byteImage);
        }

    }
}
