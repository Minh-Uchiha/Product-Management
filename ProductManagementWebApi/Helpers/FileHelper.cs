using Azure.Storage.Blobs;

namespace ProductManagementWebApi.Helpers
{
    public static class FileHelper
    {
        private readonly static string connectionString = @"DefaultEndpointsProtocol=https;AccountName=prmstorageacc;AccountKey=FQJDGs5t0nWEOkuds8k0dug5aR1QYRPK0MYoByyEA7yWTaRN+QO/wuoVDQN9rMLaanGm5idAVCW1+AStjucbuA==;EndpointSuffix=core.windows.net";
        private readonly static string containerNameForUser = "usersimg";
        private readonly static string containerNameForProduct = "productsimg";
        public static async Task<string> UploadUserImage(IFormFile Image)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerNameForUser);
            BlobClient blobClient = blobContainerClient.GetBlobClient(Image.FileName);
            MemoryStream memoryStream = new MemoryStream();
            await Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }
        public static async Task<string> UploadProductImage(IFormFile Image)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerNameForProduct);
            BlobClient blobClient = blobContainerClient.GetBlobClient(Image.FileName);
            MemoryStream memoryStream = new MemoryStream();
            await Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
