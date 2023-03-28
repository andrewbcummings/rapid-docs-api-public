using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SignerDetailsVM
    { 
        [Required]
        public int SigningId { get; set; }
        [Required]
        public int SigningDocumentId { get; set; }
        [Required]
        public string SignerGuid { get; set; }
        [Required]
        public string SignerIpAddress { get; set; }
        [Required]
        public IFormFile Document { get; set; }
        [Required]
        public int SignersIndex { get; set; }
    }
}
