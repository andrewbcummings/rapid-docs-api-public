using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_services.Services.TextControl
{
    public interface IThumbnailService
    {
        Task<string[]> GetThumbnail(string base64Document);
    }
}
