using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_viewmodels.ViewModels
{
    public class CustomerAccountVM
    {
        public int Id { get; set; }
        public ICollection<UserViewModel> Users { get; set; }

        public ICollection<SigningVM> Signings { get; set; }
    }
}
