using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Helpers
{
    public class ImageHelper
    {
        private readonly IConfiguration Configuration;
        private readonly string[] AllowedExtensions;
        private readonly string[] AllowedMimeTypes;

        public ImageHelper()
        {
            AllowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".svg", ".heif", ".heic" };
            AllowedMimeTypes = new string[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/tiff", "image/webp", "image/svg+xml", "image/heif", "image/heic" };
        }

        public bool IsValidImage(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            return AllowedExtensions.Contains(extension)
                && AllowedMimeTypes.Contains(image.ContentType.ToLowerInvariant());
        }

        public async Task<List<string>> SaveImages(List<IFormFile> images, string directoryPath)
        {
            var imagePaths = new List<string>();

            foreach (var image in images)
            {
                if (!IsValidImage(image))
                    throw new InvalidFormatException($"Invalid format of image {image.FileName}. Acceptable formats: {string.Join(", ", AllowedExtensions)}");
            }

            foreach (var image in images)
            {
                var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imagePaths.Add(fileName);
            }

            return imagePaths;
        }

        public async Task DeleteImages(List<string> imageNames, string directoryPath)
        {
            string[] filePaths = new string[imageNames.Count];

            for (int i = 0; i < imageNames.Count; i++)
            {
                filePaths[i] = Path.Combine(directoryPath, imageNames[i]);
                if (!File.Exists(filePaths[i])) throw new NotFoundException($"Image with name {imageNames[i]} does not exist in directory {directoryPath}.");
            }

            for (int i = 0; i < filePaths.Length; i++)
            {
                File.Delete(filePaths[i]);
            }
        }
    }

}
