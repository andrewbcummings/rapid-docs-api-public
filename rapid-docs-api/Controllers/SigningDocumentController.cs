using Microsoft.AspNetCore.Mvc;
using rapid_docs_api.Core;
using rapid_docs_viewmodels.ViewModels;
using System.Threading.Tasks;
using rapid_docs_services.Services.SigningDocuments;
using System;

namespace rapid_docs_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [VidaDocsAuthorize]
    public class SigningDocumentController : ControllerBase
    {
        private readonly ISigningDocumentService _signingDocumentService;
        public SigningDocumentController(ISigningDocumentService signingDocumentService)
        {
            this._signingDocumentService = signingDocumentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SigningDocumentVM), 200)]
        public async Task<IActionResult> UpdateSigningDocument([FromForm] SaveSigningDocumentVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signingDocumentService.UpdateSigningDocument(viewmodel);
                if (response == null)
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

        [HttpGet("{signingDocumentId}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetSigningDocument(int signingDocumentId)
        {
            var response = await this._signingDocumentService.GetSigningDocument(signingDocumentId);
            if (response == null)
            {
                return StatusCode(500, "We encountered and error trying to retrieve the document content.");
            }
            return Ok(response);
        }
    }
}
