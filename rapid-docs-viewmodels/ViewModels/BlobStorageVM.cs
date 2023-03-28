using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_viewmodels.ViewModels
{
    public class BlobStorageVM : BaseVM
    {
        public Guid Guid { get; set; }
        public string FileURL { get; set; }
    }
}
