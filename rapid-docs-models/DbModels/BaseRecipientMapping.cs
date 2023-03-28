using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_models.DbModels
{
    public class BaseRecipientMapping : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public long? UserId { get; set; }
        public User User { get; set; }

        [StringLength(50)]
        public string Notes { get; set; }

        [StringLength(250)]
        public string Token { get; set; }

    }
}
