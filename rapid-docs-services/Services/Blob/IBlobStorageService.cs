using Microsoft.AspNetCore.Http;

namespace rapid_docs_services.Services.Blob
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileToBlobAsync(IFormFile file, string fileGuid, string fileExtension, bool uploadToPublicContainer = false);
        Task<byte[]?> GetFileFromBlobAsync(string fileGuid, string fileExtension);
        Task<bool> DeleteFileFromBlobAsync(string fileGuid, string fileExtension);
        Task<string> UploadBase64ToBlobAsync(string base64File, string fileGuid, string fileExtension);
        Task DeleteAllFilesFromBlobAsync();
        Task<bool> DeleteFileFromPublicBlobAsync(string fileGuid, string fileExtension);
        Task DeleteAllFilesFromPublicBlobAsync();
    }
}
