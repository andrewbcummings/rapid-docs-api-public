using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SigningRecipientMappingVM : BaseVM
    {
        [Required]
        public int SigningId { get; set; }
        [Required]
        public List<string> Emails { get; set; }
        public string Notes { get; set; }
        [Required]
        public bool IsPreFilled { get; set; }
    }

    public class SigningRecipientVM
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
