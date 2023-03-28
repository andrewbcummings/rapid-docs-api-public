using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class CreateSigningVM : BaseVM
    {
        [Required]
        public string TemplateName { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        public string? Description { get; set; }
        
        [Required]
        public IFormFile File { get; set; }
        
        public bool IsTemplate { get; set; }
        public bool IsSystemTemplate { get; set; }
    }
}
