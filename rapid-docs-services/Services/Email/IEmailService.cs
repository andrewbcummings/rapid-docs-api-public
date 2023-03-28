using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_services.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string message);
    }
}
