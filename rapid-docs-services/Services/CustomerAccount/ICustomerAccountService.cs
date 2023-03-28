using Microsoft.AspNetCore.Http;

namespace rapid_docs_services.Services.CustomerAccount
{
    public interface ICustomerAccountService
    {
        Task<bool> SaveCompanyLogo(IFormFile logo);
    }
}
