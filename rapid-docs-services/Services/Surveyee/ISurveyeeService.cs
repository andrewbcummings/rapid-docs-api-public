using rapid_docs_viewmodels.ViewModels;

namespace rapid_docs_services.Services.Survyeee
{
    public interface ISurveyeeService
    {
        Task<SurveyVM?> VerifyAndGetSurvey(TokenVerificationVM tokenVM);
    }
}
