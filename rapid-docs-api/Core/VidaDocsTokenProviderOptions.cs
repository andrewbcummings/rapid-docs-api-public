using Microsoft.AspNetCore.Identity;
using System;

namespace rapid_docs_api.Core
{
    public class VidaDocsTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public VidaDocsTokenProviderOptions()
        {
            // update the defaults
            Name = "VidaDocsTokenProvider";
            TokenLifespan = TimeSpan.FromDays(15);
        }
    }
}
