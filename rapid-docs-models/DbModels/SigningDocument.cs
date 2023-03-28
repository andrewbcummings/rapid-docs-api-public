using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rapid_docs_models.DbModels
{
    public class SigningDocument : BaseModel
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string FileName { get; set; }

        [StringLength(50)]
        public string FileGuid { get; set; }

        [StringLength(10)]
        public string FileExtension { get; set; }

        [StringLength(20)]
        public string FileSize { get; set; }

        [StringLength(100)]
        public string? FileContentType { get; set; }

        public bool IsTemplate { get; set; }

        [StringLength(250)]
        public string FileUrl { get; set; }

        [ForeignKey(nameof(Signing))]
        public int SigningId { get; set; }
        public Signing Signing { get; set; }

        public int? NumberOfSignatures { get; set; }
    }
}
