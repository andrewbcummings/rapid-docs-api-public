using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SigningDocumentVM : BaseVM
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileGuid { get; set; }

        public string FileExtension { get; set; }

        public string FileSize { get; set; }

        public string? FileContentType { get; set; }
        public bool IsTemplate { get; set; }
        public string FileUrl { get; set; }

        public string? SignerGuid { get; set; }
        public string? SignerIpAddress { get; set; }
        public int? NumberOfSignatures { get; set; }
    }
}
