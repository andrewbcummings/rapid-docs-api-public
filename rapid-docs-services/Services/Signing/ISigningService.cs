using rapid_docs_viewmodels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_services.Services.SigningService
{
    public interface ISigningService
    {
        Task<SigningVM?> AddSigningDocument(CreateSigningVM signingVM);
        Task<SigningVM?> CloneSigningDocument(int signingId);
        Task<SigningVM?> UpdateSigning(SigningVM signingVM);
        Task<List<SigningVM>> GetUserSignings(bool isTemplate);

        Task<List<SigningVM>> PagedResult( string orderBy, bool ascending, int page = 0, int pageSize = 5);

        Task<List<SigningVM>> GetUserSigningsCards(bool isTemplate);
        Task<SigningVM?> GetSigning(int signingId);
        Task<int> AddRecipient(SigningRecipientMappingVM viewmodel);
        Task<bool> DeleteSigning(int signingId);
        Task<bool> SaveClientFields(SigningClientInputVM viewmodel);
        Task<bool> DeletePage(int pageId);
        Task<int> MarkAsViewed(int signingId);
    }
}
