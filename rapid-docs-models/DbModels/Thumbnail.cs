using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_models.DbModels
{
    public class Thumbnail 
    {
        public int Id { get; set; }

        public Guid FileGuid { get; set; }

        [StringLength(10)]
        public string FileExtension { get; set; }

        [StringLength(250)]
        public string FileUrl { get; set; }

        [ForeignKey(nameof(Signing))]
        public int SigningId { get; set; }
        public Signing Signing { get; set; }
    }
}
