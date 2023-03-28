using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_viewmodels.ViewModels
{
    public class ThumbnailVM
    {
        public int Id { get; set; }

        public Guid FileGuid { get; set; }

        public string FileExtension { get; set; }

        public string FileUrl { get; set; }
    }
}
