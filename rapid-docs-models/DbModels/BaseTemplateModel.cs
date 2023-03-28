using System.ComponentModel.DataAnnotations;

namespace rapid_docs_models.DbModels
{
    public class BaseTemplateModel: BaseModel
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
