using System.ComponentModel.DataAnnotations;

namespace rapid_docs_models.DbModels
{
    public class SurveyFormPage : BaseModel
    {
        public int Id { get; set; }
        public IEnumerable<SurveyInputField>? InputFields { get; set; }

        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public int PageOrder { get; set; }
    }
}
