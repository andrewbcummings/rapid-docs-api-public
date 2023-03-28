using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using rapid_docs_core.ApplicationSettings;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;
using rapid_docs_models.DbModels;
using rapid_docs_services.Services.Blob;
using rapid_docs_services.Services.Email;
using rapid_docs_services.Services.SigningService;
using rapid_docs_viewmodels.ViewModels;
using RestSharp;
using System.Text.Json;
using System.Web;

namespace rapid_docs_services.Services.Signer
{
    public class SignerService : BaseService, ISignerService
    {
        private readonly UserManager<User> _userManager;
        private readonly ISigningService _signingService;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly TextControlSettings _textControlSettings;
        private readonly SiteSettings _siteSettings;
        public SignerService(VidaDocsDbContext dbContext, IMapper mapper, VidaDocsContext ctx, UserManager<User> userManager,
            IEmailService emailService, ISigningService signingService, IBlobStorageService blobService,
            IOptions<TextControlSettings> textControlSettings, IOptions<SiteSettings> siteSettings) : base(dbContext, mapper, ctx)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this._userManager = userManager;
            this._signingService = signingService;
            this._blobService = blobService;
            this._textControlSettings = textControlSettings.Value;
            this._emailService = emailService;
            this._siteSettings = siteSettings.Value;
        }

        public async Task<SigningVM?> VerifyAndGetSigning(TokenVerificationVM tokenVM)
        {
            var user = this.dbContext.Users.AsNoTracking().FirstOrDefault(x => x.NormalizedEmail == tokenVM.Email.ToUpper());
            if (user == null)
                return null;

            var isUserVerified = await this._userManager.VerifyUserTokenAsync(user, "VidaDocsTokenProvider", "passwordless-auth", tokenVM.Token);
            if (isUserVerified)
            {
                var signingMapping = this.dbContext.SigningRecipientMappings.AsNoTracking().FirstOrDefault(x => x.Token == tokenVM.Token);

                if (signingMapping == null)
                    return null;

                var signingMappings = this.dbContext.SigningRecipientMappings.AsNoTracking().Where(x => x.SigningId == signingMapping.SigningId).ToList();
                var signerIndex = signingMappings.IndexOf(signingMappings.FirstOrDefault(x => x.Token == tokenVM.Token));

                var signingVm = await this._signingService.GetSigning(signingMapping.SigningId);
                if (signingVm != null)
                {
                    signingVm.IsPreFilled = signingMapping.IsPreFilled;
                    signingVm.SignersIndex = signerIndex;
                    signingVm.InstructionText = signingMappings?.FirstOrDefault()?.Notes ?? string.Empty;

                    var signingUser = this.dbContext.Users.AsNoTracking().Include(x => x.Company).FirstOrDefault(x => x.Id == signingMappings.First().CreatedBy);
                    if (signingUser != null && signingUser.Company != null && signingUser.Company.CompanyLogoUrl != null)
                    {
                        signingVm.CompanyLogoUrl = signingUser.Company.CompanyLogoUrl;
                    }
                }

                var marked = await this._signingService.MarkAsViewed(signingMapping.SigningId);

                return signingVm;
            }
            return null;
        }

        public async Task<List<InputFieldVM>> SaveInputFields(List<InputFieldVM> inputFieldsVM)
        {
            var inputFields = this.mapper.Map<List<InputField>>(inputFieldsVM);
            this.dbContext.InputFields.UpdateRange(inputFields);
            await this.dbContext.SaveChangesAsync();
            return this.mapper.Map<List<InputFieldVM>>(inputFields);
        }

        public async Task<bool?> SaveSignerDetails(SignerDetailsVM viewmodel)
        {
            var signingDoc = await this.dbContext.SigningDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == viewmodel.SigningDocumentId);
            if (signingDoc == null)
                return null;

            var blobResponse = await this._blobService.UploadFileToBlobAsync(viewmodel.Document, signingDoc.FileGuid, signingDoc.FileExtension);
            signingDoc.FileUrl = blobResponse;
            signingDoc.FileSize = viewmodel.Document.Length.ToString();
            signingDoc.FileContentType = viewmodel.Document.ContentType;
            this.dbContext.SigningDocuments.Update(signingDoc);

            var signingMappings = await this.dbContext.SigningRecipientMappings.AsNoTracking().Where(x => x.SigningId == signingDoc.SigningId).ToListAsync();
            signingMappings[viewmodel.SignersIndex].SignerGuid = viewmodel.SignerGuid;
            signingMappings[viewmodel.SignersIndex].SignerIpAddress = viewmodel.SignerIpAddress;

            if (viewmodel.SignersIndex < signingMappings.Count() - 1)
            {
                var emailUser = await this.dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == signingMappings[viewmodel.SignersIndex + 1].UserId);
                string recipientEmail = string.Empty;
                string recipientToken = string.Empty;
                if (emailUser != null)
                {
                    signingMappings[viewmodel.SignersIndex + 1].Token = await _userManager.GenerateUserTokenAsync(emailUser, "VidaDocsTokenProvider", "passwordless-auth");
                    recipientEmail = emailUser.Email;
                    recipientToken = signingMappings[viewmodel.SignersIndex + 1].Token;
                }

                var vidaDocsLogo = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNi4wLjIsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCAyMDIuOCAzNy41IiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCAyMDIuOCAzNy41OyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMTc2Qjg5O30NCjwvc3R5bGU+DQo8Zz4NCgk8Zz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTU3LjksMzYuOWMtMS45LDAtMy42LTAuMy01LjEtMC45Yy0xLjUtMC42LTIuOC0xLjUtMy45LTIuNmMtMS4xLTEuMS0xLjktMi41LTIuNS00LjENCgkJCWMtMC42LTEuNi0wLjktMy4zLTAuOS01LjJjMC0yLjQsMC41LTQuNSwxLjUtNi4zYzEtMS44LDIuMy0zLjIsNC4xLTQuMmMxLjctMSwzLjctMS41LDUuOS0xLjVjMS4xLDAsMi4yLDAuMiwzLjIsMC41DQoJCQljMSwwLjMsMS45LDAuOCwyLjcsMS40YzAuOCwwLjYsMS41LDEuMywxLjksMi4yaDBWMi40aDUuM3YyMS43YzAsMi42LTAuNSw0LjgtMS42LDYuN2MtMSwxLjktMi41LDMuNC00LjMsNC40DQoJCQlDNjIuNSwzNi4zLDYwLjMsMzYuOSw1Ny45LDM2Ljl6IE01Ny45LDMyLjNjMS40LDAsMi42LTAuMywzLjYtMWMxLTAuNywxLjgtMS42LDIuNC0yLjhjMC42LTEuMiwwLjktMi41LDAuOS00LjFzLTAuMy0yLjktMC45LTQuMQ0KCQkJYy0wLjYtMS4yLTEuNC0yLjEtMi40LTIuOGMtMS0wLjctMi4yLTEtMy42LTFjLTEuMywwLTIuNSwwLjMtMy42LDFjLTEuMSwwLjctMS45LDEuNi0yLjUsMi44Yy0wLjYsMS4yLTAuOSwyLjUtMC45LDQNCgkJCWMwLDEuNSwwLjMsMi45LDAuOSw0LjFjMC42LDEuMiwxLjQsMi4xLDIuNSwyLjhDNTUuNCwzMiw1Ni42LDMyLjMsNTcuOSwzMi4zeiIvPg0KCQk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMTEyLjcsMzYuOWMtMS45LDAtMy42LTAuMy01LjEtMC45Yy0xLjUtMC42LTIuOC0xLjUtMy45LTIuNmMtMS4xLTEuMS0xLjktMi41LTIuNS00LjENCgkJCWMtMC42LTEuNi0wLjktMy4zLTAuOS01LjJjMC0yLjQsMC41LTQuNSwxLjUtNi4zYzEtMS44LDIuMy0zLjIsNC4xLTQuMmMxLjctMSwzLjctMS41LDUuOS0xLjVjMS4xLDAsMi4yLDAuMiwzLjIsMC41DQoJCQljMSwwLjMsMS45LDAuOCwyLjcsMS40YzAuOCwwLjYsMS41LDEuMywxLjksMi4yaDBWMi40aDUuM3YyMS43YzAsMi42LTAuNSw0LjgtMS42LDYuN2MtMSwxLjktMi41LDMuNC00LjMsNC40DQoJCQlDMTE3LjIsMzYuMywxMTUuMSwzNi45LDExMi43LDM2Ljl6IE0xMTIuNywzMi4zYzEuNCwwLDIuNi0wLjMsMy42LTFjMS0wLjcsMS44LTEuNiwyLjQtMi44YzAuNi0xLjIsMC45LTIuNSwwLjktNC4xDQoJCQlzLTAuMy0yLjktMC45LTQuMWMtMC42LTEuMi0xLjQtMi4xLTIuNC0yLjhjLTEtMC43LTIuMi0xLTMuNi0xYy0xLjMsMC0yLjUsMC4zLTMuNiwxYy0xLjEsMC43LTEuOSwxLjYtMi41LDIuOA0KCQkJYy0wLjYsMS4yLTAuOSwyLjUtMC45LDRjMCwxLjUsMC4zLDIuOSwwLjksNC4xYzAuNiwxLjIsMS40LDIuMSwyLjUsMi44QzExMC4xLDMyLDExMS4zLDMyLjMsMTEyLjcsMzIuM3oiLz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTM4LjQsNy4yYy0xLjEsMC0yLTAuMy0yLjctMC45Yy0wLjYtMC42LTEtMS40LTEtMi40YzAtMSwwLjMtMS44LDEtMi40YzAuNi0wLjYsMS41LTAuOSwyLjYtMC45DQoJCQljMS4xLDAsMiwwLjMsMi43LDAuOUM0MS43LDIuMSw0MiwyLjksNDIsMy45YzAsMS0wLjMsMS43LTEsMi40QzQwLjQsNi45LDM5LjUsNy4yLDM4LjQsNy4yeiBNMzUuNiwzNi45VjEyLjdoNS43djI0LjJIMzUuNnoiLz4NCgkJPHBhdGggY2xhc3M9InN0MCIgZD0iTTgzLjIsMzYuOWMtMS42LDAtMy0wLjMtNC4yLTAuOWMtMS4yLTAuNi0yLjItMS41LTIuOS0yLjZDNzUuMywzMi4zLDc1LDMxLDc1LDI5LjVjMC0xLjMsMC4yLTIuNCwwLjctMy40DQoJCQljMC41LTEsMS4yLTEuOCwyLjEtMi40YzAuOS0wLjYsMi4xLTEuMSwzLjQtMS41YzEuNC0wLjMsMy0wLjUsNC44LTAuNWg3bC0wLjQsNC4xaC02LjljLTAuOCwwLTEuNiwwLjEtMi4yLDAuMg0KCQkJYy0wLjYsMC4yLTEuMiwwLjQtMS43LDAuN2MtMC41LDAuMy0wLjgsMC43LTEsMS4xYy0wLjIsMC40LTAuMywwLjktMC4zLDEuNWMwLDAuNiwwLjIsMS4yLDAuNSwxLjZjMC4zLDAuNCwwLjcsMC44LDEuMywxDQoJCQljMC41LDAuMiwxLjEsMC40LDEuOSwwLjRjMSwwLDItMC4yLDIuOS0wLjVjMC45LTAuMywxLjctMC44LDIuNC0xLjRjMC43LTAuNiwxLjMtMS4zLDEuNy0yLjFsMS4yLDMuMmMtMC42LDEuMS0xLjQsMi0yLjQsMi44DQoJCQljLTAuOSwwLjgtMiwxLjQtMy4xLDEuOUM4NS41LDM2LjYsODQuNCwzNi45LDgzLjIsMzYuOXogTTkwLjIsMzYuNFYyMC44YzAtMS4zLTAuNC0yLjQtMS4zLTMuMWMtMC45LTAuOC0yLTEuMS0zLjQtMS4xDQoJCQljLTEuMywwLTIuNSwwLjMtMy41LDAuOGMtMS4xLDAuNS0yLDEuMy0yLjksMi40bC0zLjUtMy41YzEuNS0xLjUsMy4xLTIuNyw0LjgtMy41YzEuOC0wLjgsMy43LTEuMiw1LjctMS4yYzEuOSwwLDMuNiwwLjMsNSwwLjkNCgkJCWMxLjQsMC42LDIuNSwxLjUsMy4zLDIuOGMwLjgsMS4yLDEuMiwyLjcsMS4yLDQuNXYxNi44SDkwLjJ6Ii8+DQoJCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xNDEuMiwzNi45Yy0yLjQsMC00LjYtMC41LTYuNS0xLjZjLTEuOS0xLjEtMy40LTIuNi00LjUtNC41Yy0xLjEtMS45LTEuNi00LjEtMS42LTYuNQ0KCQkJYzAtMi40LDAuNS00LjYsMS42LTYuNWMxLjEtMS45LDIuNi0zLjQsNC41LTQuNWMxLjktMS4xLDQuMS0xLjYsNi41LTEuNmMyLjQsMCw0LjYsMC41LDYuNSwxLjZjMS45LDEuMSwzLjQsMi42LDQuNSw0LjUNCgkJCWMxLjEsMS45LDEuNiw0LjEsMS42LDYuNWMwLDIuNC0wLjUsNC42LTEuNiw2LjVjLTEuMSwxLjktMi42LDMuNC00LjUsNC41QzE0NS44LDM2LjMsMTQzLjYsMzYuOSwxNDEuMiwzNi45eiBNMTQxLjIsMzEuOA0KCQkJYzEuMywwLDIuNS0wLjMsMy42LTFjMS4xLTAuNywxLjktMS42LDIuNS0yLjdzMC45LTIuNSwwLjktMy45YzAtMS41LTAuMy0yLjgtMC45LTRjLTAuNi0xLjItMS40LTIuMS0yLjUtMi43DQoJCQljLTEuMS0wLjYtMi4yLTEtMy42LTFjLTEuMywwLTIuNSwwLjMtMy42LDFjLTEuMSwwLjctMS45LDEuNi0yLjUsMi43cy0wLjksMi41LTAuOSwzLjljMCwxLjUsMC4zLDIuOCwwLjksMy45czEuNCwyLjEsMi41LDIuNw0KCQkJQzEzOC43LDMxLjUsMTM5LjksMzEuOCwxNDEuMiwzMS44eiIvPg0KCQk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMTY5LjUsMzYuOWMtMi4zLDAtNC4zLTAuNi02LjItMS43Yy0xLjgtMS4xLTMuMy0yLjYtNC4zLTQuNWMtMS4xLTEuOS0xLjYtNC4xLTEuNi02LjVzMC41LTQuNiwxLjYtNi41DQoJCQljMS4xLTEuOSwyLjUtMy40LDQuMy00LjVjMS44LTEuMSwzLjktMS43LDYuMi0xLjdjMi4yLDAsNC4xLDAuNCw2LDEuM2MxLjgsMC45LDMuMiwyLDQuMSwzLjVsLTMuMSwzLjhjLTAuNS0wLjYtMS4xLTEuMi0xLjgtMS44DQoJCQljLTAuNy0wLjUtMS41LTAuOS0yLjMtMS4zYy0wLjgtMC4zLTEuNi0wLjUtMi40LTAuNWMtMS40LDAtMi42LDAuMy0zLjcsMWMtMS4xLDAuNy0xLjksMS42LTIuNSwyLjdzLTAuOSwyLjUtMC45LDMuOQ0KCQkJYzAsMS41LDAuMywyLjgsMSwzLjljMC42LDEuMSwxLjUsMi4xLDIuNiwyLjdjMS4xLDAuNywyLjMsMSwzLjYsMWMwLjgsMCwxLjYtMC4xLDIuNC0wLjRjMC43LTAuMywxLjUtMC42LDIuMi0xLjENCgkJCWMwLjctMC41LDEuMy0xLjEsMS45LTEuOWwzLjEsMy44Yy0xLDEuNC0yLjUsMi41LTQuMywzLjNDMTczLjQsMzYuNCwxNzEuNSwzNi45LDE2OS41LDM2Ljl6Ii8+DQoJCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xOTIuMSwzNi45Yy0yLjEsMC00LjEtMC40LTUuOC0xLjFjLTEuNy0wLjctMy4xLTEuOC00LjItMy4xbDMuNy0zLjJjMC45LDEsMiwxLjgsMy4xLDIuMg0KCQkJYzEuMSwwLjUsMi4zLDAuNywzLjYsMC43YzAuNSwwLDEtMC4xLDEuNC0wLjJjMC40LTAuMSwwLjgtMC4zLDEuMS0wLjZjMC4zLTAuMiwwLjUtMC41LDAuNy0wLjljMC4yLTAuMywwLjMtMC43LDAuMy0xLjENCgkJCWMwLTAuNy0wLjMtMS4zLTAuOC0xLjhjLTAuMy0wLjItMC44LTAuNC0xLjQtMC43Yy0wLjYtMC4zLTEuNS0wLjUtMi42LTAuOGMtMS43LTAuNC0zLjEtMC45LTQuMi0xLjVjLTEuMS0wLjYtMi0xLjItMi42LTEuOQ0KCQkJYy0wLjUtMC42LTAuOS0xLjMtMS4yLTJjLTAuMi0wLjctMC40LTEuNS0wLjQtMi40YzAtMS40LDAuNC0yLjYsMS4yLTMuN2MwLjgtMS4xLDEuOS0xLjksMy4zLTIuNmMxLjQtMC42LDIuOS0wLjksNC41LTAuOQ0KCQkJYzEuMiwwLDIuNCwwLjIsMy42LDAuNWMxLjIsMC4zLDIuMiwwLjgsMy4yLDEuM2MxLDAuNiwxLjgsMS4zLDIuNSwyLjFsLTMuMiwzLjVjLTAuNi0wLjYtMS4yLTEuMS0xLjktMS41Yy0wLjctMC40LTEuNC0wLjgtMi4xLTENCgkJCWMtMC43LTAuMi0xLjQtMC40LTItMC40Yy0wLjYsMC0xLjEsMC4xLTEuNiwwLjJjLTAuNSwwLjEtMC45LDAuMy0xLjIsMC41Yy0wLjMsMC4yLTAuNSwwLjUtMC43LDAuOGMtMC4yLDAuMy0wLjMsMC43LTAuMywxLjENCgkJCWMwLDAuNCwwLjEsMC44LDAuMywxLjFjMC4yLDAuMywwLjQsMC42LDAuNywwLjhjMC4zLDAuMiwwLjgsMC41LDEuNSwwLjdjMC43LDAuMywxLjYsMC41LDIuNiwwLjhjMS41LDAuNCwyLjgsMC45LDMuOCwxLjMNCgkJCWMxLDAuNSwxLjgsMS4xLDIuNCwxLjdjMC42LDAuNiwxLDEuMiwxLjMsMS45YzAuMiwwLjcsMC40LDEuNiwwLjQsMi41YzAsMS41LTAuNCwyLjctMS4yLDMuOWMtMC44LDEuMS0xLjksMi0zLjMsMi43DQoJCQlDMTk1LjQsMzYuNSwxOTMuOSwzNi45LDE5Mi4xLDM2Ljl6Ii8+DQoJPC9nPg0KCTxnPg0KCQk8cG9seWdvbiBjbGFzcz0ic3QwIiBwb2ludHM9IjEuNCwxMi43IDE2LjIsMzYuOSAzMSwxMi43IDE2LjIsMjUuMSAJCSIvPg0KCTwvZz4NCjwvZz4NCjwvc3ZnPg0K";
                var mailBody = "<img alt=\"" + "Vidadocs Logo" + "\" src=\"date:image/svg+xml;base64, " + vidaDocsLogo + "\" />" +
                "<br />" + "Please click the following link:- <a href=\"" + _siteSettings.SiteUrl + "signer?token=" +
                HttpUtility.UrlEncode(recipientToken) + "&email=" +
                HttpUtility.UrlEncode(recipientEmail) + "\">Click Here to Sign</a>";
                var emailSent = await this._emailService.SendEmailAsync(recipientEmail, "Vida Docs", mailBody);
            }
            else if (viewmodel.SignersIndex == signingMappings.Count() - 1 && viewmodel.SigningId > 0)
            {
                var signing = this.dbContext.Signings.AsNoTracking().FirstOrDefault(x => x.Id == viewmodel.SigningId);
                if (signing != null)
                {
                    signing.IsCompleted = true;
                    this.dbContext.Signings.Update(signing);
                }
            }

            this.dbContext.SigningRecipientMappings.UpdateRange(signingMappings);
            var res = await this.dbContext.SaveChangesAsync();
            return res > 0;
        }

        public async Task<string> GetDocumentWithValues(int signingId)
        {
            var signing = this.dbContext.Signings.Include(x => x.Documents).Include(x => x.SigningForm)
                .ThenInclude(x => x.SigningFormPages).ThenInclude(x => x.InputFields).FirstOrDefault(x => x.Id == signingId);
            var signingDocument = signing?.Documents?.FirstOrDefault();

            if (signing == null || signingDocument == null)
                return string.Empty;

            var fileBytes = await this._blobService.GetFileFromBlobAsync(signingDocument.FileGuid, signingDocument.FileExtension);
            if (fileBytes != null)
            {
                string base64 = Convert.ToBase64String(fileBytes);
                var docUpdateRequest = new
                {
                    MergeData = new Dictionary<string, string>(),
                    Template = base64
                };

                var inputFields = signing.SigningForm?.SigningFormPages?.FirstOrDefault()?.InputFields;
                if (inputFields == null || inputFields.Count() == 0)
                    return base64;

                foreach (var field in inputFields)
                {
                    string key = field.Label.Replace(" ", "") + field.Name.Replace("-", "");
                    docUpdateRequest.MergeData.Add(key, field.Value);
                }

                var client = new RestClient(_textControlSettings.BaseUrl + "documentprocessing/document/merge?returnFormat=TX");
                var request = new RestRequest()
                {
                    Method = Method.Post
                }
                .AddHeader("Content-Type", "application/json")
                .AddBody(docUpdateRequest);

                RestResponse response = await client.ExecutePostAsync(request);
                if (response.Content != null)
                {
                    var docContent = JsonSerializer.Deserialize<string[]>(response.Content);
                    if (docContent != null && docContent.Count() > 0)
                    {
                        return docContent.First();
                    }
                }
            }
            return string.Empty;
        }
    }
}
