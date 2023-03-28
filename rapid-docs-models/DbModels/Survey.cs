namespace rapid_docs_models.DbModels
{
    public class Survey : BaseTemplateModel
    {
        public SurveyForm? SurveyForm { get; set; }

        public bool IsTemplate { get; set; }

        public bool IsSystemTemplate { get; set; }
        public DateTime DateSent { get; set; }
        
        public DateTime DateLastOpened { get; set; }

        public int NumberOfTimesOpened { get; set; }

        public bool UserHasStarted { get; set; }
    }
}
