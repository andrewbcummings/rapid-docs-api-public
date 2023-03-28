using rapid_docs_viewmodels.ViewModels;

namespace rapid_docs_services.Services.SurveyService
{
    public interface ISurveyService
    {
        Task<SurveyVM?> AddSurvey(SurveyVM surveyVM);
        Task<SurveyVM?> UpdateSurvey(SurveyVM surveyVM);
        Task<SurveyVM?> GetSurvey(int surveyId);
        Task<int> MarkAsViewed(int surveyId);
        Task<bool> DeletePage(int pageId);
        Task<int> AddRecipient(SurveyRecipientMappingVM viewmodel);
        Task<bool> SaveSurveyFields(SurveyClientInputVM viewmodel);
    }
}
