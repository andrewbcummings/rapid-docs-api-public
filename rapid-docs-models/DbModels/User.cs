using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_models.DbModels
{
    public class User : IdentityUser<long>
    {
        [StringLength(250)]
        public string? SocialId { get; set; }
        [StringLength(100)]
        public string? NickName { get; set; }
        [StringLength(250)]
        public string? ProfilePicture { get; set; }

        [ForeignKey(nameof(Company))]
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
