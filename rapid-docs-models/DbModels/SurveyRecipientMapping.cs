using System.ComponentModel.DataAnnotations.Schema;

namespace rapid_docs_models.DbModels
{
    public class SurveyRecipientMapping : BaseRecipientMapping
    {
        [ForeignKey(nameof(Survey))]
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
    }
}
