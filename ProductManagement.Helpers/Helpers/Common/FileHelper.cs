using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace ProductManagementWebApi.Helpers.Common
{
    public static class FileHelper
    {
        public static async Task<string> UploadUserImage(IFormFile Image, string connectionString, string containerName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(Image.FileName);
            MemoryStream memoryStream = new MemoryStream();
            await Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }
        public static async Task<string> UploadProductImage(IFormFile Image, string connectionString, string containerName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(Image.FileName);
            MemoryStream memoryStream = new MemoryStream();
            await Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
