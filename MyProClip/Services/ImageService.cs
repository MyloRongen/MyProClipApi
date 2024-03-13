using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MyProClip.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile thumbnailFile)
        {
            ValidateImage(thumbnailFile);
            return await GenerateThumbnailNameAsync(thumbnailFile);
        }

        private void ValidateImage(IFormFile thumbnailFile)
        {
            if (thumbnailFile == null || thumbnailFile.Length == 0)
            {
                throw new Exception("No file provided or file is empty.");
            }

            string extension = Path.GetExtension(thumbnailFile.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif")
            {
                throw new Exception("Invalid file format. Only JPG, JPEG, PNG, and GIF files are allowed.");
            }
        }

        private async Task<string> GenerateThumbnailNameAsync(IFormFile thumbnailFile)
        {
            Guid myuuid = Guid.NewGuid();

            string thumbnailName = new String(Path.GetFileNameWithoutExtension(thumbnailFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            thumbnailName = thumbnailName + DateTime.Now.ToString("yymmssfff") + myuuid.ToString() + Path.GetExtension(thumbnailFile.FileName);
            var thumbnailPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Thumbnails", thumbnailName);

            using (var fileStream = new FileStream(thumbnailPath, FileMode.Create))
            {
                await thumbnailFile.CopyToAsync(fileStream);
            }

            return thumbnailName;
        }
    }
}
