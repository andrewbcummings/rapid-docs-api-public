using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rapid_docs_models.DbModels;

namespace rapid_docs_api.Core
{
    public class VidaDocsTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
    where TUser : User
    {
        public VidaDocsTokenProvider(IDataProtectionProvider dataProtectionProvider,
        IOptions<VidaDocsTokenProviderOptions> options, ILogger<VidaDocsTokenProvider<TUser>> logger)
        : base(dataProtectionProvider, options, logger)
        { }
    }
}
