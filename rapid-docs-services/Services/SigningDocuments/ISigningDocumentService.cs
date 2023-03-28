using rapid_docs_viewmodels.ViewModels;

namespace rapid_docs_services.Services.SigningDocuments
{
    public interface ISigningDocumentService
    {
        Task<SigningDocumentVM?> UpdateSigningDocument(SaveSigningDocumentVM viewmodel);
        Task<string?> GetSigningDocument(int signingDocumentId);
    }
}
