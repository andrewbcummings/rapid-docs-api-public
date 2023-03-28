using Microsoft.AspNetCore.Mvc;
using rapid_docs_services.Services.Signer;
using rapid_docs_services.Services.SigningDocuments;
using rapid_docs_services.Services.SigningService;
using rapid_docs_viewmodels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignerController : ControllerBase
    {
        private readonly ISignerService _signerService;
        private readonly ISigningService _signingService;
        private readonly ISigningDocumentService _signingDocumentService;
        public SignerController(ISignerService signerService, ISigningService signingService, ISigningDocumentService signingDocumentService)
        {
            this._signerService = signerService;
            this._signingService = signingService;
            this._signingDocumentService = signingDocumentService;
        }

        [HttpPost("verify")]
        [ProducesResponseType(typeof(SigningVM), 200)]
        public async Task<IActionResult> VerifyAndGetSigning(TokenVerificationVM tokenVM)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signerService.VerifyAndGetSigning(tokenVM);
                if (response == null)
                {
                    return StatusCode(500, "We encountered and error trying to retrieve the signing record.");
                }

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("signing")]
        [ProducesResponseType(typeof(SigningVM), 200)]
        public async Task<IActionResult> GetSigning(int signingId)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signingService.GetSigning(signingId);
                if (response == null)
                {
                    return StatusCode(500, "We encountered and error trying to retrieve the signing record.");
                }

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("inputValues")]
        [ProducesResponseType(typeof(List<InputFieldVM>), 200)]
        public async Task<IActionResult> SaveInputFields(List<InputFieldVM> inputFieldsVM)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signerService.SaveInputFields(inputFieldsVM);
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("documentWithValues/{signingId}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetDocumentWithValues(int signingId)
        {
            var response = await this._signerService.GetDocumentWithValues(signingId);
            if (response == null)
            {
                return StatusCode(500, "We encountered and error trying to retrieve the document content.");
            }
            return Ok(response);
        }

        [HttpPut("signerDetails")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SaveSignerDetails([FromForm]SignerDetailsVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signerService.SaveSignerDetails(viewmodel);
                if (response == null)
                {
                    return StatusCode(500, "We encountered and error trying to retrieve the document record.");
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
