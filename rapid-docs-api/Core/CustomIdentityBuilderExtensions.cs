using Microsoft.AspNetCore.Identity;

namespace rapid_docs_api.Core
{
    public static class CustomIdentityBuilderExtensions
    {
        public static IdentityBuilder AddVidaDocsTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var provider = typeof(VidaDocsTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider("VidaDocsTokenProvider", provider);
        }
    }
}
