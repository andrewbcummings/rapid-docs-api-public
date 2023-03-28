using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rapid_docs_models.DbModels
{
    public class SigningRecipientMapping : BaseRecipientMapping
    {
        [ForeignKey(nameof(Signing))]
        public int SigningId { get; set; }
        public Signing Signing { get; set; }

        public bool IsPreFilled { get; set; }

        [StringLength(50)]
        public string? SignerGuid { get; set; }

        [StringLength(20)]
        public string? SignerIpAddress { get; set; }
    }
}
