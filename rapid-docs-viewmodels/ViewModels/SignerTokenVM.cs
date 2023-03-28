using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class TokenVerificationVM
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
