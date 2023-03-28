using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SaveSigningDocumentVM : BaseVM
    {
        [Required]
        public int SigningId { get; set; }

        [Required]
        public IFormFile File { get; set; }
        [Required]
        public int NumberOfSignatures { get; set; }
    }
}
