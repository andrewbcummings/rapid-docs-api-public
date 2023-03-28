using System.ComponentModel.DataAnnotations;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SigningClientInputVM
    {
        [Required]
        public List<InputFieldVM> InputFields { get; set; }
        [Required]
        public int SigningId { get; set; }
    }
}
