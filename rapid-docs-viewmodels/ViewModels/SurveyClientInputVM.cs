using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SurveyClientInputVM
    {
        [Required]
        public List<InputFieldVM> InputFields { get; set; }
        [Required]
        public int SurveyId { get; set; }
        [Required]
        public int SurveyeeIndex { get; set; }
    }
}
