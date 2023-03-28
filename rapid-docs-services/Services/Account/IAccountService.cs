using rapid_docs_viewmodels.ViewModels;

namespace rapid_docs_services.Services.Account
{
    public interface IAccountService
    {
        Task<int> UserDetails(UserViewModel user);
    }
}
