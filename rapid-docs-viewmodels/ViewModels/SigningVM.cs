using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SigningVM : BaseVM
    {
        public int Id { get; set; }

        public BaseTemplateFormVM? TemplateForm { get; set; }

        public string TemplateName { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public bool IsTemplate { get; set; }

        public bool IsSystemTemplate { get; set; }
       
        public string ApiVersion { get; set; }
        

        public DateTime DateSent { get; set; }

        public DateTime DateLastOpened { get; set; }

        public DateTime CreatedDate { get; set; }

        public int NumberOfTimesOpened { get; set; }

        public bool UserHasStarted { get; set; }

        public List<SigningDocumentVM> Documents { get; set; }

        public ThumbnailVM? Thumbnail { get; set; }

        public string? EmailToken { get; set; }
        public string? CompanyLogoUrl { get; set; }
        public bool IsPreFilled { get; set; }
        public int? SignersIndex { get; set; }
        public string? InstructionText { get; set; }
        public bool IsCompleted { get; set; }
    }
}
