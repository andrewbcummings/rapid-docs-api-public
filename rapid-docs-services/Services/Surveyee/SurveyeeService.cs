using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;
using rapid_docs_models.DbModels;
using rapid_docs_services.Services.SurveyService;
using rapid_docs_viewmodels.ViewModels;

namespace rapid_docs_services.Services.Survyeee
{
    public class SurveyeeService : BaseService, ISurveyeeService
    {
        private readonly UserManager<User> _userManager;
        private readonly ISurveyService _surveyService;
        public SurveyeeService(VidaDocsDbContext dbContext, IMapper mapper, VidaDocsContext ctx, UserManager<User> userManager,
             ISurveyService surveyService) : base(dbContext, mapper, ctx)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this._userManager = userManager;
            this._surveyService = surveyService;
        }

        public async Task<SurveyVM?> VerifyAndGetSurvey(TokenVerificationVM tokenVM)
        {
            var user = this.dbContext.Users.AsNoTracking().FirstOrDefault(x => x.NormalizedEmail == tokenVM.Email.ToUpper());
            if (user == null)
                return null;

            var isUserVerified = await this._userManager.VerifyUserTokenAsync(user, "VidaDocsTokenProvider", "passwordless-auth", tokenVM.Token);
            if (isUserVerified)
            {
                var surveyMapping = this.dbContext.SurveyRecipientMappings.AsNoTracking().FirstOrDefault(x => x.Token == tokenVM.Token);

                if (surveyMapping == null)
                    return null;

                var surveyMappings = this.dbContext.SurveyRecipientMappings.AsNoTracking().Where(x => x.SurveyId == surveyMapping.SurveyId).ToList();
                var surveyIndex = surveyMappings.IndexOf(surveyMappings.FirstOrDefault(x => x.Token == tokenVM.Token));

                var surveyVm = await this._surveyService.GetSurvey(surveyMapping.SurveyId);
                if (surveyVm != null)
                {
                    surveyVm.SurveyeeIndex = surveyIndex;
                    surveyVm.InstructionText = surveyMappings?.FirstOrDefault()?.Notes ?? string.Empty;

                    var signingUser = this.dbContext.Users.AsNoTracking().Include(x => x.Company).FirstOrDefault(x => x.Id == surveyMappings.First().CreatedBy);
                    if (signingUser != null && signingUser.Company != null && signingUser.Company.CompanyLogoUrl != null)
                    {
                        surveyVm.CompanyLogoUrl = signingUser.Company.CompanyLogoUrl;
                    }
                }
                var marked = await this._surveyService.MarkAsViewed(surveyMapping.SurveyId);
                return surveyVm;
            }
            return null;
        }
    }
}
