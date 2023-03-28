using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using rapid_docs_core.ApplicationSettings;
using System.Fabric.Management.ServiceModel;

namespace rapid_docs_services.Services.Blob
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobStorageSettings blobSettings;

        public BlobStorageService(IOptions<BlobStorageSettings> blobSettings)
        {
            this.blobSettings = blobSettings.Value;
        }

        public async Task<string> UploadFileToBlobAsync(IFormFile file, string fileGuid, string fileExtension, bool uploadToPublicContainer = false)
        {
            string connectionString = string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2};EndpointSuffix={3}", blobSettings.Protocol, blobSettings.AccountName, blobSettings.AccountKey, blobSettings.EndpointSuffix);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = uploadToPublicContainer ? blobSettings.PublicContainerName : blobSettings.ContainerName;
            var blobContainer = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainer.GetBlobClient(fileGuid + "." + fileExtension);
            await blobClient.UploadAsync(file.OpenReadStream(), overwrite: true);
            return blobClient.Uri.AbsoluteUri.ToString();
        }

        public async Task<string> UploadBase64ToBlobAsync(string base64File, string fileGuid, string fileExtension)
        {
            byte[] data = Convert.FromBase64String(base64File);
            var fileContent = new StreamContent(new MemoryStream(data));

            string connectionString = string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2};EndpointSuffix={3}", blobSettings.Protocol, blobSettings.AccountName, blobSettings.AccountKey, blobSettings.EndpointSuffix);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainer = blobServiceClient.GetBlobContainerClient(blobSettings.PublicContainerName);
            var blobClient = blobContainer.GetBlobClient(fileGuid + "." + fileExtension);
            await blobClient.UploadAsync(fileContent.ReadAsStream(), overwrite: true);
            return blobClient.Uri.AbsoluteUri.ToString();
        }


        public async Task<byte[]?> GetFileFromBlobAsync(string fileGuid, string fileExtension)
        {
            string connectionString = string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2};EndpointSuffix={3}", blobSettings.Protocol, blobSettings.AccountName, blobSettings.AccountKey, blobSettings.EndpointSuffix);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainer = blobServiceClient.GetBlobContainerClient(blobSettings.ContainerName);
            var blobClient = blobContainer.GetBlobClient(fileGuid + "." + fileExtension);
            if (await blobClient.ExistsAsync())
            {
                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                return stream.ToArray();
            }
            return null;
        }

        public async Task<bool> DeleteFileFromBlobAsync(string fileGuid, string fileExtension)
        {
            string connectionString = string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2};EndpointSuffix={3}", blobSettings.Protocol, blobSettings.AccountName, blobSettings.AccountKey, blobSettings.EndpointSuffix);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainer = blobServiceClient.GetBlobContainerClient(blobSettings.ContainerName);
            var blobClient = blobContainer.GetBlobClient(fileGuid + "." + fileExtension);
            var isExists = await blobClient.ExistsAsync();
            if (isExists)
            {
                var result = await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
                return result.Value;
            }
            return false;
        }

        public async Task DeleteAllFilesFromBlobAsync()
        {
            string connectionString = string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2};EndpointSuffix={3}", blobSettings.Protocol, blobSettings.AccountName, blobSettings.AccountKey, blobSettings.EndpointSuffix);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainer = blobServiceClient.GetBlobContainerClient(blobSettings.ContainerName);
            await foreach (var blob in blobContainer.GetBlobsAsync())
            {
                var blobClient = blobContainer.GetBlobClient(blob.Name);
                await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
            }
        }

        public async Task<bool> DeleteFileFromPublicBlobAsync(string fileGuid, string fileExtension)
        {
            string connectionString = string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2};EndpointSuffix={3}", blobSettings.Protocol, blobSettings.AccountName, blobSettings.AccountKey, blobSettings.EndpointSuffix);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainer = blobServiceClient.GetBlobContainerClient(blobSettings.PublicContainerName);
            var blobClient = blobContainer.GetBlobClient(fileGuid + "." + fileExtension);
            var isExists = await blobClient.ExistsAsync();
            if (isExists)
            {
                var result = await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
                return result.Value;
            }
            return false;
        }

        public async Task DeleteAllFilesFromPublicBlobAsync()
        {
            string connectionString = string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2};EndpointSuffix={3}", blobSettings.Protocol, blobSettings.AccountName, blobSettings.AccountKey, blobSettings.EndpointSuffix);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainer = blobServiceClient.GetBlobContainerClient(blobSettings.PublicContainerName);
            await foreach (var blob in blobContainer.GetBlobsAsync())
            {
                var blobClient = blobContainer.GetBlobClient(blob.Name);
                await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
            }
        }
    }
}
