using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using rapid_docs_core.ApplicationSettings;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;
using rapid_docs_models.DbModels;
using rapid_docs_services.Services.Email;
using rapid_docs_viewmodels.ViewModels;
using System.Web;

namespace rapid_docs_services.Services.SurveyService
{
    public class SurveyService : BaseService, ISurveyService
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly SiteSettings _siteSettings;

        public SurveyService(VidaDocsDbContext dbContext, IMapper mapper, VidaDocsContext ctx, IOptions<SiteSettings> siteSettings,
            IEmailService emailService, UserManager<User> userManager) : base(dbContext, mapper, ctx)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.ctx = ctx;
            this._userManager = userManager;
            this._emailService = emailService;
            this._siteSettings = siteSettings.Value;
        }

        public async Task<SurveyVM?> AddSurvey(SurveyVM surveyVM)
        {
            var surveyModel = this.mapper.Map<Survey>(surveyVM);
            this.dbContext.Surveys.Add(surveyModel);
            var response = await this.dbContext.SaveChangesAsync();
            if (response == 0)
                return null;
            return this.mapper.Map<SurveyVM>(surveyModel);
        }

        public async Task<SurveyVM?> UpdateSurvey(SurveyVM surveyVM)
        {
            this.dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var surveyModel = await dbContext.Surveys.AsNoTracking().FirstOrDefaultAsync(x => x.Id == surveyVM.Id);
            surveyModel = this.mapper.Map<Survey>(surveyVM);
            this.dbContext.Surveys.Update(surveyModel);
            var response = await this.dbContext.SaveChangesAsync();
            if (response == 0)
                return null;
            return this.mapper.Map<SurveyVM>(surveyModel);
        }

        public async Task<SurveyVM?> GetSurvey(int surveyId)
        {
            var survey = await dbContext.Surveys.AsNoTracking().Include(x => x.SurveyForm).ThenInclude(x => x.SurveyFormPages)
                .ThenInclude(x => x.InputFields).ThenInclude(x => x.Options).FirstOrDefaultAsync(x => x.Id == surveyId);
            if (survey == null)
                return null;

            var surveyVM = this.mapper.Map<SurveyVM>(survey);
            return surveyVM;
        }

        public async Task<int> MarkAsViewed(int surveyId)
        {
            var survey = await dbContext.Surveys.AsNoTracking().FirstOrDefaultAsync(x => x.Id == surveyId);
            survey.NumberOfTimesOpened = survey.NumberOfTimesOpened + 1;
            survey.DateLastOpened = DateTime.UtcNow;
            this.dbContext.Surveys.Update(survey);
            var result = await this.dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeletePage(int pageId)
        {
            var surveyFormPage = await this.dbContext.SurveyFormPages.AsNoTracking()
            .Include(x => x.InputFields).ThenInclude(x => x.Options).FirstOrDefaultAsync(x => x.Id == pageId);

            if (surveyFormPage == null)
                return false;

            if (surveyFormPage.InputFields != null && surveyFormPage.InputFields.Count() > 0)
            {
                var options = surveyFormPage.InputFields.Where(x => x.Options != null && x.Options.Count() > 0).SelectMany(x => x.Options);
                if (options.Any())
                    this.dbContext.SurveyInputOptions.RemoveRange(options);
            }
            this.dbContext.SurveyFormPages.Remove(surveyFormPage);
            var res = await this.dbContext.SaveChangesAsync();
            return res > 0;
        }

        public async Task<int> AddRecipient(SurveyRecipientMappingVM viewmodel)
        {
            var survey = await this.dbContext.Surveys.AsNoTracking().Include(x => x.SurveyForm).ThenInclude(x => x.SurveyFormPages)
                            .ThenInclude(x => x.InputFields).ThenInclude(x => x.Options).FirstOrDefaultAsync(x => x.Id == viewmodel.SurveyId);

            if (survey == null)
                return 0;

            survey.Id = 0;
            survey.IsTemplate = false;

            // Copy signing form
            if (survey.SurveyForm != null)
            {
                survey.SurveyForm.Id = 0;
                if (survey.SurveyForm.SurveyFormPages != null && survey.SurveyForm.SurveyFormPages.Count() > 0)
                    survey.SurveyForm.SurveyFormPages.ToList().ForEach(page =>
                    {
                        page.Id = 0;
                        if (page.InputFields != null && page.InputFields.Count() > 0)
                            page.InputFields.ToList().ForEach(input =>
                            {
                                input.Id = Guid.NewGuid();
                                if (input.Options != null && input.Options.Count() > 0)
                                    input.Options.ToList().ForEach(option => { option.Id = 0; });
                            });
                    });
            }

            this.dbContext.Surveys.Add(survey);
            this.dbContext.SaveChanges();

            var surveyMappings = new List<SurveyRecipientMapping>();

            foreach (var email in viewmodel.Emails)
            {
                var user = await this.dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedEmail == email.ToUpper());
                var surveyMapping = new SurveyRecipientMapping
                {
                    Id = 0,
                    Notes = viewmodel.Notes,
                    SurveyId = survey.Id,
                    Token = String.Empty
                };

                if (user == null)
                {
                    user = new User
                    {
                        Email = email,
                        NormalizedEmail = email.ToUpper(),
                        EmailConfirmed = false,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = email,
                        NormalizedUserName = email.ToUpper(),
                    };
                    surveyMapping.User = user;
                }
                else
                {
                    surveyMapping.UserId = user.Id;
                    if (user.SecurityStamp == null)
                    {
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        this.dbContext.Users.Update(user);
                    }
                }
                surveyMappings.Add(surveyMapping);
            }

            this.dbContext.SurveyRecipientMappings.AddRange(surveyMappings);
            var result = await this.dbContext.SaveChangesAsync();
            var emailUser = await this.dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == surveyMappings.First().UserId);
            string recipientEmail = string.Empty;
            string recipientToken = string.Empty;

            if (emailUser != null)
            {
                surveyMappings[0].Token = await _userManager.GenerateUserTokenAsync(emailUser, "VidaDocsTokenProvider", "passwordless-auth");
                recipientEmail = emailUser.Email;
                recipientToken = surveyMappings[0].Token;

                this.dbContext.SurveyRecipientMappings.UpdateRange(surveyMappings);
                await this.dbContext.SaveChangesAsync();
            }

            if (result > 0)
            {
                // TODO : replace email link with correct url
                var vidaDocsLogo = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNi4wLjIsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCAyMDIuOCAzNy41IiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCAyMDIuOCAzNy41OyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMTc2Qjg5O30NCjwvc3R5bGU+DQo8Zz4NCgk8Zz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTU3LjksMzYuOWMtMS45LDAtMy42LTAuMy01LjEtMC45Yy0xLjUtMC42LTIuOC0xLjUtMy45LTIuNmMtMS4xLTEuMS0xLjktMi41LTIuNS00LjENCgkJCWMtMC42LTEuNi0wLjktMy4zLTAuOS01LjJjMC0yLjQsMC41LTQuNSwxLjUtNi4zYzEtMS44LDIuMy0zLjIsNC4xLTQuMmMxLjctMSwzLjctMS41LDUuOS0xLjVjMS4xLDAsMi4yLDAuMiwzLjIsMC41DQoJCQljMSwwLjMsMS45LDAuOCwyLjcsMS40YzAuOCwwLjYsMS41LDEuMywxLjksMi4yaDBWMi40aDUuM3YyMS43YzAsMi42LTAuNSw0LjgtMS42LDYuN2MtMSwxLjktMi41LDMuNC00LjMsNC40DQoJCQlDNjIuNSwzNi4zLDYwLjMsMzYuOSw1Ny45LDM2Ljl6IE01Ny45LDMyLjNjMS40LDAsMi42LTAuMywzLjYtMWMxLTAuNywxLjgtMS42LDIuNC0yLjhjMC42LTEuMiwwLjktMi41LDAuOS00LjFzLTAuMy0yLjktMC45LTQuMQ0KCQkJYy0wLjYtMS4yLTEuNC0yLjEtMi40LTIuOGMtMS0wLjctMi4yLTEtMy42LTFjLTEuMywwLTIuNSwwLjMtMy42LDFjLTEuMSwwLjctMS45LDEuNi0yLjUsMi44Yy0wLjYsMS4yLTAuOSwyLjUtMC45LDQNCgkJCWMwLDEuNSwwLjMsMi45LDAuOSw0LjFjMC42LDEuMiwxLjQsMi4xLDIuNSwyLjhDNTUuNCwzMiw1Ni42LDMyLjMsNTcuOSwzMi4zeiIvPg0KCQk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMTEyLjcsMzYuOWMtMS45LDAtMy42LTAuMy01LjEtMC45Yy0xLjUtMC42LTIuOC0xLjUtMy45LTIuNmMtMS4xLTEuMS0xLjktMi41LTIuNS00LjENCgkJCWMtMC42LTEuNi0wLjktMy4zLTAuOS01LjJjMC0yLjQsMC41LTQuNSwxLjUtNi4zYzEtMS44LDIuMy0zLjIsNC4xLTQuMmMxLjctMSwzLjctMS41LDUuOS0xLjVjMS4xLDAsMi4yLDAuMiwzLjIsMC41DQoJCQljMSwwLjMsMS45LDAuOCwyLjcsMS40YzAuOCwwLjYsMS41LDEuMywxLjksMi4yaDBWMi40aDUuM3YyMS43YzAsMi42LTAuNSw0LjgtMS42LDYuN2MtMSwxLjktMi41LDMuNC00LjMsNC40DQoJCQlDMTE3LjIsMzYuMywxMTUuMSwzNi45LDExMi43LDM2Ljl6IE0xMTIuNywzMi4zYzEuNCwwLDIuNi0wLjMsMy42LTFjMS0wLjcsMS44LTEuNiwyLjQtMi44YzAuNi0xLjIsMC45LTIuNSwwLjktNC4xDQoJCQlzLTAuMy0yLjktMC45LTQuMWMtMC42LTEuMi0xLjQtMi4xLTIuNC0yLjhjLTEtMC43LTIuMi0xLTMuNi0xYy0xLjMsMC0yLjUsMC4zLTMuNiwxYy0xLjEsMC43LTEuOSwxLjYtMi41LDIuOA0KCQkJYy0wLjYsMS4yLTAuOSwyLjUtMC45LDRjMCwxLjUsMC4zLDIuOSwwLjksNC4xYzAuNiwxLjIsMS40LDIuMSwyLjUsMi44QzExMC4xLDMyLDExMS4zLDMyLjMsMTEyLjcsMzIuM3oiLz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTM4LjQsNy4yYy0xLjEsMC0yLTAuMy0yLjctMC45Yy0wLjYtMC42LTEtMS40LTEtMi40YzAtMSwwLjMtMS44LDEtMi40YzAuNi0wLjYsMS41LTAuOSwyLjYtMC45DQoJCQljMS4xLDAsMiwwLjMsMi43LDAuOUM0MS43LDIuMSw0MiwyLjksNDIsMy45YzAsMS0wLjMsMS43LTEsMi40QzQwLjQsNi45LDM5LjUsNy4yLDM4LjQsNy4yeiBNMzUuNiwzNi45VjEyLjdoNS43djI0LjJIMzUuNnoiLz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTgzLjIsMzYuOWMtMS42LDAtMy0wLjMtNC4yLTAuOWMtMS4yLTAuNi0yLjItMS41LTIuOS0yLjZDNzUuMywzMi4zLDc1LDMxLDc1LDI5LjVjMC0xLjMsMC4yLTIuNCwwLjctMy40DQoJCQljMC41LTEsMS4yLTEuOCwyLjEtMi40YzAuOS0wLjYsMi4xLTEuMSwzLjQtMS41YzEuNC0wLjMsMy0wLjUsNC44LTAuNWg3bC0wLjQsNC4xaC02LjljLTAuOCwwLTEuNiwwLjEtMi4yLDAuMg0KCQkJYy0wLjYsMC4yLTEuMiwwLjQtMS43LDAuN2MtMC41LDAuMy0wLjgsMC43LTEsMS4xYy0wLjIsMC40LTAuMywwLjktMC4zLDEuNWMwLDAuNiwwLjIsMS4yLDAuNSwxLjZjMC4zLDAuNCwwLjcsMC44LDEuMywxDQoJCQljMC41LDAuMiwxLjEsMC40LDEuOSwwLjRjMSwwLDItMC4yLDIuOS0wLjVjMC45LTAuMywxLjctMC44LDIuNC0xLjRjMC43LTAuNiwxLjMtMS4zLDEuNy0yLjFsMS4yLDMuMmMtMC42LDEuMS0xLjQsMi0yLjQsMi44DQoJCQljLTAuOSwwLjgtMiwxLjQtMy4xLDEuOUM4NS41LDM2LjYsODQuNCwzNi45LDgzLjIsMzYuOXogTTkwLjIsMzYuNFYyMC44YzAtMS4zLTAuNC0yLjQtMS4zLTMuMWMtMC45LTAuOC0yLTEuMS0zLjQtMS4xDQoJCQljLTEuMywwLTIuNSwwLjMtMy41LDAuOGMtMS4xLDAuNS0yLDEuMy0yLjksMi40bC0zLjUtMy41YzEuNS0xLjUsMy4xLTIuNyw0LjgtMy41YzEuOC0wLjgsMy43LTEuMiw1LjctMS4yYzEuOSwwLDMuNiwwLjMsNSwwLjkNCgkJCWMxLjQsMC42LDIuNSwxLjUsMy4zLDIuOGMwLjgsMS4yLDEuMiwyLjcsMS4yLDQuNXYxNi44SDkwLjJ6Ii8+DQoJCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xNDEuMiwzNi45Yy0yLjQsMC00LjYtMC41LTYuNS0xLjZjLTEuOS0xLjEtMy40LTIuNi00LjUtNC41Yy0xLjEtMS45LTEuNi00LjEtMS42LTYuNQ0KCQkJYzAtMi40LDAuNS00LjYsMS42LTYuNWMxLjEtMS45LDIuNi0zLjQsNC41LTQuNWMxLjktMS4xLDQuMS0xLjYsNi41LTEuNmMyLjQsMCw0LjYsMC41LDYuNSwxLjZjMS45LDEuMSwzLjQsMi42LDQuNSw0LjUNCgkJCWMxLjEsMS45LDEuNiw0LjEsMS42LDYuNWMwLDIuNC0wLjUsNC42LTEuNiw2LjVjLTEuMSwxLjktMi42LDMuNC00LjUsNC41QzE0NS44LDM2LjMsMTQzLjYsMzYuOSwxNDEuMiwzNi45eiBNMTQxLjIsMzEuOA0KCQkJYzEuMywwLDIuNS0wLjMsMy42LTFjMS4xLTAuNywxLjktMS42LDIuNS0yLjdzMC45LTIuNSwwLjktMy45YzAtMS41LTAuMy0yLjgtMC45LTRjLTAuNi0xLjItMS40LTIuMS0yLjUtMi43DQoJCQljLTEuMS0wLjYtMi4yLTEtMy42LTFjLTEuMywwLTIuNSwwLjMtMy42LDFjLTEuMSwwLjctMS45LDEuNi0yLjUsMi43cy0wLjksMi41LTAuOSwzLjljMCwxLjUsMC4zLDIuOCwwLjksMy45czEuNCwyLjEsMi41LDIuNw0KCQkJQzEzOC43LDMxLjUsMTM5LjksMzEuOCwxNDEuMiwzMS44eiIvPg0KCQk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMTY5LjUsMzYuOWMtMi4zLDAtNC4zLTAuNi02LjItMS43Yy0xLjgtMS4xLTMuMy0yLjYtNC4zLTQuNWMtMS4xLTEuOS0xLjYtNC4xLTEuNi02LjVzMC41LTQuNiwxLjYtNi41DQoJCQljMS4xLTEuOSwyLjUtMy40LDQuMy00LjVjMS44LTEuMSwzLjktMS43LDYuMi0xLjdjMi4yLDAsNC4xLDAuNCw2LDEuM2MxLjgsMC45LDMuMiwyLDQuMSwzLjVsLTMuMSwzLjhjLTAuNS0wLjYtMS4xLTEuMi0xLjgtMS44DQoJCQljLTAuNy0wLjUtMS41LTAuOS0yLjMtMS4zYy0wLjgtMC4zLTEuNi0wLjUtMi40LTAuNWMtMS40LDAtMi42LDAuMy0zLjcsMWMtMS4xLDAuNy0xLjksMS42LTIuNSwyLjdzLTAuOSwyLjUtMC45LDMuOQ0KCQkJYzAsMS41LDAuMywyLjgsMSwzLjljMC42LDEuMSwxLjUsMi4xLDIuNiwyLjdjMS4xLDAuNywyLjMsMSwzLjYsMWMwLjgsMCwxLjYtMC4xLDIuNC0wLjRjMC43LTAuMywxLjUtMC42LDIuMi0xLjENCgkJCWMwLjctMC41LDEuMy0xLjEsMS45LTEuOWwzLjEsMy44Yy0xLDEuNC0yLjUsMi41LTQuMywzLjNDMTczLjQsMzYuNCwxNzEuNSwzNi45LDE2OS41LDM2Ljl6Ii8+DQoJCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xOTIuMSwzNi45Yy0yLjEsMC00LjEtMC40LTUuOC0xLjFjLTEuNy0wLjctMy4xLTEuOC00LjItMy4xbDMuNy0zLjJjMC45LDEsMiwxLjgsMy4xLDIuMg0KCQkJYzEuMSwwLjUsMi4zLDAuNywzLjYsMC43YzAuNSwwLDEtMC4xLDEuNC0wLjJjMC40LTAuMSwwLjgtMC4zLDEuMS0wLjZjMC4zLTAuMiwwLjUtMC41LDAuNy0wLjljMC4yLTAuMywwLjMtMC43LDAuMy0xLjENCgkJCWMwLTAuNy0wLjMtMS4zLTAuOC0xLjhjLTAuMy0wLjItMC44LTAuNC0xLjQtMC43Yy0wLjYtMC4zLTEuNS0wLjUtMi42LTAuOGMtMS43LTAuNC0zLjEtMC45LTQuMi0xLjVjLTEuMS0wLjYtMi0xLjItMi42LTEuOQ0KCQkJYy0wLjUtMC42LTAuOS0xLjMtMS4yLTJjLTAuMi0wLjctMC40LTEuNS0wLjQtMi40YzAtMS40LDAuNC0yLjYsMS4yLTMuN2MwLjgtMS4xLDEuOS0xLjksMy4zLTIuNmMxLjQtMC42LDIuOS0wLjksNC41LTAuOQ0KCQkJYzEuMiwwLDIuNCwwLjIsMy42LDAuNWMxLjIsMC4zLDIuMiwwLjgsMy4yLDEuM2MxLDAuNiwxLjgsMS4zLDIuNSwyLjFsLTMuMiwzLjVjLTAuNi0wLjYtMS4yLTEuMS0xLjktMS41Yy0wLjctMC40LTEuNC0wLjgtMi4xLTENCgkJCWMtMC43LTAuMi0xLjQtMC40LTItMC40Yy0wLjYsMC0xLjEsMC4xLTEuNiwwLjJjLTAuNSwwLjEtMC45LDAuMy0xLjIsMC41Yy0wLjMsMC4yLTAuNSwwLjUtMC43LDAuOGMtMC4yLDAuMy0wLjMsMC43LTAuMywxLjENCgkJCWMwLDAuNCwwLjEsMC44LDAuMywxLjFjMC4yLDAuMywwLjQsMC42LDAuNywwLjhjMC4zLDAuMiwwLjgsMC41LDEuNSwwLjdjMC43LDAuMywxLjYsMC41LDIuNiwwLjhjMS41LDAuNCwyLjgsMC45LDMuOCwxLjMNCgkJCWMxLDAuNSwxLjgsMS4xLDIuNCwxLjdjMC42LDAuNiwxLDEuMiwxLjMsMS45YzAuMiwwLjcsMC40LDEuNiwwLjQsMi41YzAsMS41LTAuNCwyLjctMS4yLDMuOWMtMC44LDEuMS0xLjksMi0zLjMsMi43DQoJCQlDMTk1LjQsMzYuNSwxOTMuOSwzNi45LDE5Mi4xLDM2Ljl6Ii8+DQoJPC9nPg0KCTxnPg0KCQk8cG9seWdvbiBjbGFzcz0ic3QwIiBwb2ludHM9IjEuNCwxMi43IDE2LjIsMzYuOSAzMSwxMi43IDE2LjIsMjUuMSAJCSIvPg0KCTwvZz4NCjwvZz4NCjwvc3ZnPg0K";
                var mailBody = "<img alt=\"" + "Vidadocs Logo" + "\" src=\"date:image/svg+xml;base64, " + vidaDocsLogo + "\" />" +
                "<br />" + "Please click the following link:- <a href=\"" + _siteSettings.SiteUrl + "surveyee?token=" +
                HttpUtility.UrlEncode(recipientToken) + "&email=" +
                HttpUtility.UrlEncode(recipientEmail) + "\">Click Here to fill the survey form</a>";
                var emailSent = await this._emailService.SendEmailAsync(recipientEmail, "Vida Docs", mailBody);
            }
            return surveyMappings.First().SurveyId;
        }

        public async Task<bool> SaveSurveyFields(SurveyClientInputVM viewmodel)
        {
            var emailSent = false;
            var surveyMappings = await this.dbContext.SurveyRecipientMappings.AsNoTracking().Where(x => x.SurveyId == viewmodel.SurveyId).ToListAsync();

            if (surveyMappings == null)
                return false;

            var inputFields = this.mapper.Map<List<SurveyInputField>>(viewmodel.InputFields);
            this.dbContext.SurveyInputFields.UpdateRange(inputFields);

            if (viewmodel.SurveyeeIndex < surveyMappings.Count() - 1)
            {
                var emailUser = await this.dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == surveyMappings[viewmodel.SurveyeeIndex + 1].UserId);
                string recipientEmail = string.Empty;
                string recipientToken = string.Empty;
                if (emailUser != null)
                {
                    surveyMappings[viewmodel.SurveyeeIndex + 1].Token = await _userManager.GenerateUserTokenAsync(emailUser, "VidaDocsTokenProvider", "passwordless-auth");
                    recipientEmail = emailUser.Email;
                    recipientToken = surveyMappings[viewmodel.SurveyeeIndex + 1].Token;
                }
                var vidaDocsLogo = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNi4wLjIsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCAyMDIuOCAzNy41IiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCAyMDIuOCAzNy41OyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMTc2Qjg5O30NCjwvc3R5bGU+DQo8Zz4NCgk8Zz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTU3LjksMzYuOWMtMS45LDAtMy42LTAuMy01LjEtMC45Yy0xLjUtMC42LTIuOC0xLjUtMy45LTIuNmMtMS4xLTEuMS0xLjktMi41LTIuNS00LjENCgkJCWMtMC42LTEuNi0wLjktMy4zLTAuOS01LjJjMC0yLjQsMC41LTQuNSwxLjUtNi4zYzEtMS44LDIuMy0zLjIsNC4xLTQuMmMxLjctMSwzLjctMS41LDUuOS0xLjVjMS4xLDAsMi4yLDAuMiwzLjIsMC41DQoJCQljMSwwLjMsMS45LDAuOCwyLjcsMS40YzAuOCwwLjYsMS41LDEuMywxLjksMi4yaDBWMi40aDUuM3YyMS43YzAsMi42LTAuNSw0LjgtMS42LDYuN2MtMSwxLjktMi41LDMuNC00LjMsNC40DQoJCQlDNjIuNSwzNi4zLDYwLjMsMzYuOSw1Ny45LDM2Ljl6IE01Ny45LDMyLjNjMS40LDAsMi42LTAuMywzLjYtMWMxLTAuNywxLjgtMS42LDIuNC0yLjhjMC42LTEuMiwwLjktMi41LDAuOS00LjFzLTAuMy0yLjktMC45LTQuMQ0KCQkJYy0wLjYtMS4yLTEuNC0yLjEtMi40LTIuOGMtMS0wLjctMi4yLTEtMy42LTFjLTEuMywwLTIuNSwwLjMtMy42LDFjLTEuMSwwLjctMS45LDEuNi0yLjUsMi44Yy0wLjYsMS4yLTAuOSwyLjUtMC45LDQNCgkJCWMwLDEuNSwwLjMsMi45LDAuOSw0LjFjMC42LDEuMiwxLjQsMi4xLDIuNSwyLjhDNTUuNCwzMiw1Ni42LDMyLjMsNTcuOSwzMi4zeiIvPg0KCQk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMTEyLjcsMzYuOWMtMS45LDAtMy42LTAuMy01LjEtMC45Yy0xLjUtMC42LTIuOC0xLjUtMy45LTIuNmMtMS4xLTEuMS0xLjktMi41LTIuNS00LjENCgkJCWMtMC42LTEuNi0wLjktMy4zLTAuOS01LjJjMC0yLjQsMC41LTQuNSwxLjUtNi4zYzEtMS44LDIuMy0zLjIsNC4xLTQuMmMxLjctMSwzLjctMS41LDUuOS0xLjVjMS4xLDAsMi4yLDAuMiwzLjIsMC41DQoJCQljMSwwLjMsMS45LDAuOCwyLjcsMS40YzAuOCwwLjYsMS41LDEuMywxLjksMi4yaDBWMi40aDUuM3YyMS43YzAsMi42LTAuNSw0LjgtMS42LDYuN2MtMSwxLjktMi41LDMuNC00LjMsNC40DQoJCQlDMTE3LjIsMzYuMywxMTUuMSwzNi45LDExMi43LDM2Ljl6IE0xMTIuNywzMi4zYzEuNCwwLDIuNi0wLjMsMy42LTFjMS0wLjcsMS44LTEuNiwyLjQtMi44YzAuNi0xLjIsMC45LTIuNSwwLjktNC4xDQoJCQlzLTAuMy0yLjktMC45LTQuMWMtMC42LTEuMi0xLjQtMi4xLTIuNC0yLjhjLTEtMC43LTIuMi0xLTMuNi0xYy0xLjMsMC0yLjUsMC4zLTMuNiwxYy0xLjEsMC43LTEuOSwxLjYtMi41LDIuOA0KCQkJYy0wLjYsMS4yLTAuOSwyLjUtMC45LDRjMCwxLjUsMC4zLDIuOSwwLjksNC4xYzAuNiwxLjIsMS40LDIuMSwyLjUsMi44QzExMC4xLDMyLDExMS4zLDMyLjMsMTEyLjcsMzIuM3oiLz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTM4LjQsNy4yYy0xLjEsMC0yLTAuMy0yLjctMC45Yy0wLjYtMC42LTEtMS40LTEtMi40YzAtMSwwLjMtMS44LDEtMi40YzAuNi0wLjYsMS41LTAuOSwyLjYtMC45DQoJCQljMS4xLDAsMiwwLjMsMi43LDAuOUM0MS43LDIuMSw0MiwyLjksNDIsMy45YzAsMS0wLjMsMS43LTEsMi40QzQwLjQsNi45LDM5LjUsNy4yLDM4LjQsNy4yeiBNMzUuNiwzNi45VjEyLjdoNS43djI0LjJIMzUuNnoiLz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTgzLjIsMzYuOWMtMS42LDAtMy0wLjMtNC4yLTAuOWMtMS4yLTAuNi0yLjItMS41LTIuOS0yLjZDNzUuMywzMi4zLDc1LDMxLDc1LDI5LjVjMC0xLjMsMC4yLTIuNCwwLjctMy40DQoJCQljMC41LTEsMS4yLTEuOCwyLjEtMi40YzAuOS0wLjYsMi4xLTEuMSwzLjQtMS41YzEuNC0wLjMsMy0wLjUsNC44LTAuNWg3bC0wLjQsNC4xaC02LjljLTAuOCwwLTEuNiwwLjEtMi4yLDAuMg0KCQkJYy0wLjYsMC4yLTEuMiwwLjQtMS43LDAuN2MtMC41LDAuMy0wLjgsMC43LTEsMS4xYy0wLjIsMC40LTAuMywwLjktMC4zLDEuNWMwLDAuNiwwLjIsMS4yLDAuNSwxLjZjMC4zLDAuNCwwLjcsMC44LDEuMywxDQoJCQljMC41LDAuMiwxLjEsMC40LDEuOSwwLjRjMSwwLDItMC4yLDIuOS0wLjVjMC45LTAuMywxLjctMC44LDIuNC0xLjRjMC43LTAuNiwxLjMtMS4zLDEuNy0yLjFsMS4yLDMuMmMtMC42LDEuMS0xLjQsMi0yLjQsMi44DQoJCQljLTAuOSwwLjgtMiwxLjQtMy4xLDEuOUM4NS41LDM2LjYsODQuNCwzNi45LDgzLjIsMzYuOXogTTkwLjIsMzYuNFYyMC44YzAtMS4zLTAuNC0yLjQtMS4zLTMuMWMtMC45LTAuOC0yLTEuMS0zLjQtMS4xDQoJCQljLTEuMywwLTIuNSwwLjMtMy41LDAuOGMtMS4xLDAuNS0yLDEuMy0yLjksMi40bC0zLjUtMy41YzEuNS0xLjUsMy4xLTIuNyw0LjgtMy41YzEuOC0wLjgsMy43LTEuMiw1LjctMS4yYzEuOSwwLDMuNiwwLjMsNSwwLjkNCgkJCWMxLjQsMC42LDIuNSwxLjUsMy4zLDIuOGMwLjgsMS4yLDEuMiwyLjcsMS4yLDQuNXYxNi44SDkwLjJ6Ii8+DQoJCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xNDEuMiwzNi45Yy0yLjQsMC00LjYtMC41LTYuNS0xLjZjLTEuOS0xLjEtMy40LTIuNi00LjUtNC41Yy0xLjEtMS45LTEuNi00LjEtMS42LTYuNQ0KCQkJYzAtMi40LDAuNS00LjYsMS42LTYuNWMxLjEtMS45LDIuNi0zLjQsNC41LTQuNWMxLjktMS4xLDQuMS0xLjYsNi41LTEuNmMyLjQsMCw0LjYsMC41LDYuNSwxLjZjMS45LDEuMSwzLjQsMi42LDQuNSw0LjUNCgkJCWMxLjEsMS45LDEuNiw0LjEsMS42LDYuNWMwLDIuNC0wLjUsNC42LTEuNiw2LjVjLTEuMSwxLjktMi42LDMuNC00LjUsNC41QzE0NS44LDM2LjMsMTQzLjYsMzYuOSwxNDEuMiwzNi45eiBNMTQxLjIsMzEuOA0KCQkJYzEuMywwLDIuNS0wLjMsMy42LTFjMS4xLTAuNywxLjktMS42LDIuNS0yLjdzMC45LTIuNSwwLjktMy45YzAtMS41LTAuMy0yLjgtMC45LTRjLTAuNi0xLjItMS40LTIuMS0yLjUtMi43DQoJCQljLTEuMS0wLjYtMi4yLTEtMy42LTFjLTEuMywwLTIuNSwwLjMtMy42LDFjLTEuMSwwLjctMS45LDEuNi0yLjUsMi43cy0wLjksMi41LTAuOSwzLjljMCwxLjUsMC4zLDIuOCwwLjksMy45czEuNCwyLjEsMi41LDIuNw0KCQkJQzEzOC43LDMxLjUsMTM5LjksMzEuOCwxNDEuMiwzMS44eiIvPg0KCQk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMTY5LjUsMzYuOWMtMi4zLDAtNC4zLTAuNi02LjItMS43Yy0xLjgtMS4xLTMuMy0yLjYtNC4zLTQuNWMtMS4xLTEuOS0xLjYtNC4xLTEuNi02LjVzMC41LTQuNiwxLjYtNi41DQoJCQljMS4xLTEuOSwyLjUtMy40LDQuMy00LjVjMS44LTEuMSwzLjktMS43LDYuMi0xLjdjMi4yLDAsNC4xLDAuNCw2LDEuM2MxLjgsMC45LDMuMiwyLDQuMSwzLjVsLTMuMSwzLjhjLTAuNS0wLjYtMS4xLTEuMi0xLjgtMS44DQoJCQljLTAuNy0wLjUtMS41LTAuOS0yLjMtMS4zYy0wLjgtMC4zLTEuNi0wLjUtMi40LTAuNWMtMS40LDAtMi42LDAuMy0zLjcsMWMtMS4xLDAuNy0xLjksMS42LTIuNSwyLjdzLTAuOSwyLjUtMC45LDMuOQ0KCQkJYzAsMS41LDAuMywyLjgsMSwzLjljMC42LDEuMSwxLjUsMi4xLDIuNiwyLjdjMS4xLDAuNywyLjMsMSwzLjYsMWMwLjgsMCwxLjYtMC4xLDIuNC0wLjRjMC43LTAuMywxLjUtMC42LDIuMi0xLjENCgkJCWMwLjctMC41LDEuMy0xLjEsMS45LTEuOWwzLjEsMy44Yy0xLDEuNC0yLjUsMi41LTQuMywzLjNDMTczLjQsMzYuNCwxNzEuNSwzNi45LDE2OS41LDM2Ljl6Ii8+DQoJCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xOTIuMSwzNi45Yy0yLjEsMC00LjEtMC40LTUuOC0xLjFjLTEuNy0wLjctMy4xLTEuOC00LjItMy4xbDMuNy0zLjJjMC45LDEsMiwxLjgsMy4xLDIuMg0KCQkJYzEuMSwwLjUsMi4zLDAuNywzLjYsMC43YzAuNSwwLDEtMC4xLDEuNC0wLjJjMC40LTAuMSwwLjgtMC4zLDEuMS0wLjZjMC4zLTAuMiwwLjUtMC41LDAuNy0wLjljMC4yLTAuMywwLjMtMC43LDAuMy0xLjENCgkJCWMwLTAuNy0wLjMtMS4zLTAuOC0xLjhjLTAuMy0wLjItMC44LTAuNC0xLjQtMC43Yy0wLjYtMC4zLTEuNS0wLjUtMi42LTAuOGMtMS43LTAuNC0zLjEtMC45LTQuMi0xLjVjLTEuMS0wLjYtMi0xLjItMi42LTEuOQ0KCQkJYy0wLjUtMC42LTAuOS0xLjMtMS4yLTJjLTAuMi0wLjctMC40LTEuNS0wLjQtMi40YzAtMS40LDAuNC0yLjYsMS4yLTMuN2MwLjgtMS4xLDEuOS0xLjksMy4zLTIuNmMxLjQtMC42LDIuOS0wLjksNC41LTAuOQ0KCQkJYzEuMiwwLDIuNCwwLjIsMy42LDAuNWMxLjIsMC4zLDIuMiwwLjgsMy4yLDEuM2MxLDAuNiwxLjgsMS4zLDIuNSwyLjFsLTMuMiwzLjVjLTAuNi0wLjYtMS4yLTEuMS0xLjktMS41Yy0wLjctMC40LTEuNC0wLjgtMi4xLTENCgkJCWMtMC43LTAuMi0xLjQtMC40LTItMC40Yy0wLjYsMC0xLjEsMC4xLTEuNiwwLjJjLTAuNSwwLjEtMC45LDAuMy0xLjIsMC41Yy0wLjMsMC4yLTAuNSwwLjUtMC43LDAuOGMtMC4yLDAuMy0wLjMsMC43LTAuMywxLjENCgkJCWMwLDAuNCwwLjEsMC44LDAuMywxLjFjMC4yLDAuMywwLjQsMC42LDAuNywwLjhjMC4zLDAuMiwwLjgsMC41LDEuNSwwLjdjMC43LDAuMywxLjYsMC41LDIuNiwwLjhjMS41LDAuNCwyLjgsMC45LDMuOCwxLjMNCgkJCWMxLDAuNSwxLjgsMS4xLDIuNCwxLjdjMC42LDAuNiwxLDEuMiwxLjMsMS45YzAuMiwwLjcsMC40LDEuNiwwLjQsMi41YzAsMS41LTAuNCwyLjctMS4yLDMuOWMtMC44LDEuMS0xLjksMi0zLjMsMi43DQoJCQlDMTk1LjQsMzYuNSwxOTMuOSwzNi45LDE5Mi4xLDM2Ljl6Ii8+DQoJPC9nPg0KCTxnPg0KCQk8cG9seWdvbiBjbGFzcz0ic3QwIiBwb2ludHM9IjEuNCwxMi43IDE2LjIsMzYuOSAzMSwxMi43IDE2LjIsMjUuMSAJCSIvPg0KCTwvZz4NCjwvZz4NCjwvc3ZnPg0K";
                var mailBody = "<img alt=\"" + "Vidadocs Logo" + "\" src=\"date:image/svg+xml;base64, " + vidaDocsLogo + "\" />" +
                 "<br />" + "Please click the following link:- <a href=\"" + _siteSettings.SiteUrl + "surveyee?token=" +
                HttpUtility.UrlEncode(recipientToken) + "&email=" +
                HttpUtility.UrlEncode(recipientEmail) + "\">Click Here to fill the survey form</a>";
                emailSent = await this._emailService.SendEmailAsync(recipientEmail, "Vida Docs", mailBody);
                if (emailSent)
                {
                    this.dbContext.SurveyRecipientMappings.UpdateRange(surveyMappings);
                }
            }

            var result = await this.dbContext.SaveChangesAsync();
            return result > 0;
        }

    }
}
