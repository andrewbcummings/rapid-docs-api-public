namespace rapid_docs_viewmodels.ViewModels
{
    public class SurveyVM : BaseVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public BaseTemplateFormVM? TemplateForm { get; set; }

        public bool IsTemplate { get; set; }

        public bool IsSystemTemplate { get; set; }
       
        public DateTime DateSent { get; set; }

        public DateTime DateLastOpened { get; set; }

        public int NumberOfTimesOpened { get; set; }

        public bool UserHasStarted { get; set; }

        public string? CompanyLogoUrl { get; set; }
        public int? SurveyeeIndex { get; set; }
        public string? InstructionText { get; set; }
    }
}
