using rapid_docs_viewmodels.ViewModels;

namespace rapid_docs_services.Services.Signer
{
    public interface ISignerService
    {
        Task<SigningVM?> VerifyAndGetSigning(TokenVerificationVM tokenVM);
        Task<List<InputFieldVM>> SaveInputFields(List<InputFieldVM> inputFieldsVM);
        Task<bool?> SaveSignerDetails(SignerDetailsVM viewmodel);
        Task<string> GetDocumentWithValues(int signingDocumentId);
    }
}
