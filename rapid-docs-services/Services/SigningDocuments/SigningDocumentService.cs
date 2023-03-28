using AutoMapper;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;
using rapid_docs_models.DbModels;
using rapid_docs_services.Services.Blob;
using rapid_docs_viewmodels.ViewModels;

namespace rapid_docs_services.Services.SigningDocuments
{
    public class SigningDocumentService : BaseService, ISigningDocumentService
    {
        private readonly IBlobStorageService _blobService;
        public SigningDocumentService(VidaDocsDbContext dbContext, IMapper mapper, VidaDocsContext ctx, IBlobStorageService blobService) : base(dbContext, mapper, ctx)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.ctx = ctx;
            this._blobService = blobService;
        }

        public async Task<SigningDocumentVM?> UpdateSigningDocument(SaveSigningDocumentVM viewmodel)
        {
            var signingDocument = this.dbContext.SigningDocuments.FirstOrDefault(x => x.SigningId == viewmodel.SigningId);
            if (signingDocument == null || viewmodel.File == null)
                return null;
            else
            {
                var blobResponse = await this._blobService.UploadFileToBlobAsync(viewmodel.File, signingDocument.FileGuid, signingDocument.FileExtension);
                signingDocument.FileUrl = blobResponse;
                signingDocument.FileSize = viewmodel.File.Length.ToString();
                signingDocument.FileContentType = viewmodel.File.ContentType;
                signingDocument.NumberOfSignatures = viewmodel.NumberOfSignatures;
                this.dbContext.SigningDocuments.Update(signingDocument);
                var resposne = await this.dbContext.SaveChangesAsync();
                return this.mapper.Map<SigningDocumentVM>(signingDocument);
            }
        }

        public async Task<string?> GetSigningDocument(int signingDocumentId)
        {
            var signingDocument = this.dbContext.SigningDocuments.FirstOrDefault(x => x.Id == signingDocumentId);
            if (signingDocument == null)
                return null;

            var fileBytes = await this._blobService.GetFileFromBlobAsync(signingDocument.FileGuid, signingDocument.FileExtension);
            if (fileBytes != null)
            {
                string base64 = Convert.ToBase64String(fileBytes);
                return base64;
            }
            return null;
        }
    }
}
