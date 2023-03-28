using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_models.DbModels
{
    public class Company
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string CompanyLogoUrl { get; set; }
        [StringLength(100)]
        public string CompanyLogoGuid { get; set; }
        [StringLength(10)]
        public string CompanyLogoExtension { get; set; }

        //public ICollection<User> Users { get; set; }

        public ICollection<Signing> Signings { get; set; }
    }
}
