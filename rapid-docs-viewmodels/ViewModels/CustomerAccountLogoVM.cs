using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class CustomerAccountLogoVM
    {
        [Required]
        public IFormFile Logo { get; set; }
    }
}
