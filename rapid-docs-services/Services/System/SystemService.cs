using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;
using rapid_docs_services.Services.Blob;
using rapid_docs_services.Services.SigningService;

namespace rapid_docs_services.Services.System
{
    public class SystemService : BaseService, ISystemService
    {
        private readonly ISigningService _signingService;
        private readonly IBlobStorageService _blobStorageService;
        public SystemService(VidaDocsDbContext dbContext, IMapper mapper, VidaDocsContext ctx,
            ISigningService signingService, IBlobStorageService blobStorageService) : base(dbContext, mapper, ctx)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.ctx = ctx;
            this._signingService = signingService;
            this._blobStorageService = blobStorageService;
        }

        public async Task CleanSignings()
        {
            var signings = await this.dbContext.Signings.AsNoTracking()
                .Include(x => x.Documents).Include(x => x.Thumbnail).AsNoTracking().ToListAsync();
            if (signings != null && signings.Count > 0)
            {
                foreach (var signing in signings)
                {
                    if (signing != null)
                    {
                        var signingDoc = signing?.Documents?.FirstOrDefault();
                        if (signingDoc != null)
                            await this._blobStorageService.DeleteFileFromBlobAsync(signingDoc.FileGuid, signingDoc.FileExtension);
                        if (signing.Thumbnail != null)
                            await this._blobStorageService.DeleteFileFromPublicBlobAsync(signing.Thumbnail.FileGuid.ToString(), signing.Thumbnail.FileExtension);
                        await this._signingService.DeleteSigning(signing.Id);
                    }
                }
            }
        }

        public async Task CleanBlobStorage()
        {
            await this._blobStorageService.DeleteAllFilesFromBlobAsync();
            await this._blobStorageService.DeleteAllFilesFromPublicBlobAsync();
        }
    }
}
