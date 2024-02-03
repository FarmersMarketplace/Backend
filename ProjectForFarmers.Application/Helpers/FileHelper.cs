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
    public class FileHelper
    {
        private readonly string[] AllowedImagesExtensions;
        private readonly string[] AllowedImagesMimeTypes;

        public FileHelper()
        {
            AllowedImagesExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".svg", ".heif", ".heic" };
            AllowedImagesMimeTypes = new string[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/tiff", "image/webp", "image/svg+xml", "image/heif", "image/heic" };
        }

        public bool IsValidImage(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            return AllowedImagesExtensions.Contains(extension)
                && AllowedImagesMimeTypes.Contains(image.ContentType.ToLowerInvariant());
        }

        public async Task<List<string>> SaveImages(List<IFormFile> images, string directoryPath)
        {
            foreach (var image in images)
            {
                if (!IsValidImage(image))
                    throw new InvalidFormatException($"Invalid format of image {image.FileName}. Acceptable formats: {string.Join(", ", AllowedImagesExtensions)}");
            }

            var imagePaths = await SaveFiles(images, directoryPath);

            return imagePaths;
        }

        public async Task<List<string>> SaveFiles(List<IFormFile> files, string directoryPath)
        {
            var filesNames = new List<string>();

            foreach (var file in files)
            {
                var fileName = await SaveFile(file, directoryPath);

                filesNames.Add(fileName);
            }

            return filesNames;
        }

        public async Task<string> SaveFile(IFormFile file, string directoryPath)
        {
            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task DeleteFiles(List<string> filesNames, string directoryPath)
        {
            string[] filePaths = new string[filesNames.Count];

            for (int i = 0; i < filesNames.Count; i++)
            {
                filePaths[i] = Path.Combine(directoryPath, filesNames[i]);
                if (!File.Exists(filePaths[i])) throw new NotFoundException($"File with name {filesNames[i]} does not exist in directory {directoryPath}.");
            }

            for (int i = 0; i < filePaths.Length; i++)
            {
                File.Delete(filePaths[i]);
            }
        }
    }

}
