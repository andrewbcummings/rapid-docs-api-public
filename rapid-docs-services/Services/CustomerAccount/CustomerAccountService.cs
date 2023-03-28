using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;
using rapid_docs_models.DbModels;
using rapid_docs_services.Services.Blob;

namespace rapid_docs_services.Services.CustomerAccount
{
    public class CustomerAccountService : BaseService, ICustomerAccountService
    {
        private readonly IBlobStorageService _blobService;

        public CustomerAccountService(VidaDocsDbContext dbContext, IMapper mapper, VidaDocsContext ctx,
            IBlobStorageService blobService) : base(dbContext, mapper, ctx)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.ctx = ctx;
            this._blobService = blobService;
        }

        public async Task<bool> SaveCompanyLogo(IFormFile logo)
        {
            var user = this.dbContext.Users.AsNoTracking().Include(x => x.Company).FirstOrDefault(x => x.Id == this.ctx.UserId);
            if (user == null)
                return false;

            if (user.Company == null)
                user.Company = new Company();

            if (logo != null && user.Company != null)
            {
                var fileExtension = logo.FileName.Split('.').Last();
                var guid = Guid.NewGuid();
                var blobResponse = await this._blobService.UploadFileToBlobAsync(logo, guid.ToString(), fileExtension, true);

                user.Company.CompanyLogoUrl = blobResponse;
                user.Company.CompanyLogoGuid = guid.ToString();
                user.Company.CompanyLogoExtension = fileExtension;
                this.dbContext.Users.Update(user);
                var response = await this.dbContext.SaveChangesAsync();
                return response > 0;
            }

            return false;
        }
    }
}
