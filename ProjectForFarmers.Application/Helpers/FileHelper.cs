using Microsoft.AspNetCore.Http;
using ProjectForFarmers.Application.Exceptions;

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
            if(images == null) return new List<string>();

            foreach (var image in images)
            {
                if (!IsValidImage(image))
                {
                    string acceptableFormats = string.Join(", ", AllowedImagesExtensions);
                    string message = $"Invalid format of image {image.FileName}. Acceptable formats: {acceptableFormats}.";
                    string userFacingMessage = CultureHelper.Exception("InvalidImageFormat", image.FileName, acceptableFormats);
                    throw new NotFoundException(message, userFacingMessage);
                }
            }

            var imagePaths = await SaveFiles(images, directoryPath);

            return imagePaths;
        }

        public async Task<string> CopyFile(string sourceFilePath, string destinationDirectory)
        {
            if (!File.Exists(sourceFilePath))
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string message = $"Source file with path {sourceFilePath} was not found.";
                string userFacingMessage = CultureHelper.Exception("SourceFileNotFound", fileName);

                throw new NotFoundException(message, userFacingMessage);
            }

            if (!Directory.Exists(destinationDirectory))
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string message = $"Directory with path {sourceFilePath} was not found.";
                string userFacingMessage = CultureHelper.Exception("An error occurred while creating the file.");

                throw new NotFoundException(message, userFacingMessage);
            }

            string fileExtension = Path.GetExtension(sourceFilePath);
            string newFileName = $"{Guid.NewGuid()}.{fileExtension}";
            string destinationFilePath = Path.Combine(destinationDirectory, newFileName);

            File.Copy(sourceFilePath, destinationFilePath);

            return newFileName;
        }

        public async Task<List<string>> SaveFiles(List<IFormFile> files, string directoryPath)
        {
            if (files == null) return new List<string>();

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
                if (!File.Exists(filePaths[i]))
                {
                    string message = $"File with name {filesNames[i]} does not exist in directory {directoryPath}.";
                    string userFacingMessage = CultureHelper.Exception("FileNotExist", filesNames[i].ToString(), directoryPath);
                    throw new NotFoundException(message, userFacingMessage);
                }
            }

            for (int i = 0; i < filePaths.Length; i++)
            {
                File.Delete(filePaths[i]);
            }
        }

        public void DeleteFile(string fileName, string directoryPath)
        {
            var filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

}
