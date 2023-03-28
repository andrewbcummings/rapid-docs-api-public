using System.ComponentModel.DataAnnotations.Schema;

namespace rapid_docs_models.DbModels
{
    public class Signing : BaseTemplateModel
    {
        public SigningForm? SigningForm { get; set; }

        public string TemplateName { get; set; }

        public bool IsTemplate { get; set; }
        public bool IsSystemTemplate { get; set; }

        public string ApiVersion { get; set; }

        public int DateCreated { get; set; }

        public DateTime DateSent { get; set; }

        public DateTime DateLastOpened { get; set; }

        public int NumberOfTimesOpened { get; set; }

        public bool UserHasStarted { get; set; }
        public ICollection<SigningDocument> Documents { get; set; }

        public Thumbnail Thumbnail { get; set; }
        public string? EmailToken { get; set; }
        
        public bool IsCompleted { get; set; }
    }
}
