using System.ComponentModel.DataAnnotations;

namespace rapid_docs_models.DbModels
{
    public class SurveyInputField : BaseModel
    {
        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Label { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        [StringLength(250)]
        public string DefaultValue { get; set; }

        public bool Required { get; set; }

        public IEnumerable<SurveyInputOption>? Options { get; set; }

        public int Order { get; set; }

        public Guid Id { get; set; }
    }
}
