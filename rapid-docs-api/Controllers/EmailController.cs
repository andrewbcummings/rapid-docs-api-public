using Microsoft.AspNetCore.Mvc;
using rapid_docs_api.Core;
using rapid_docs_services.Services.Email;
using rapid_docs_viewmodels.ViewModels;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [VidaDocsAuthorize]
    public class EmailController : ControllerBase
    {
        private IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPut("Signing")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SendSigning(SigningVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._emailService.SendEmailAsync("", "", "");
                if (response)
                {
                    return StatusCode(500, "Data not Saved");
                }

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
