using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SurveyRecipientMappingVM:BaseVM
    {
        [Required]
        public int SurveyId { get; set; }
        
        [Required]
        public List<string> Emails { get; set; }
        
        public string Notes { get; set; }
    }
}
