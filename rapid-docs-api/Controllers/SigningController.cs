using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rapid_docs_api.Core;
using rapid_docs_models.DbModels;
using rapid_docs_services.Services.Blob;
using rapid_docs_services.Services.SigningService;
using rapid_docs_viewmodels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [VidaDocsAuthorize]
    public class SigningController : ControllerBase
    {
        private readonly ISigningService _signingService;
        public SigningController(ISigningService signingService)
        {
            this._signingService = signingService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SigningVM), 200)]
        public async Task<IActionResult> AddSigningDocument([FromForm] CreateSigningVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signingService.AddSigningDocument(viewmodel);
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

        [HttpGet("Clone")]
        [ProducesResponseType(typeof(SigningVM), 200)]
        public async Task<IActionResult> CloneSigningDocument(int signingId)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signingService.CloneSigningDocument(signingId);
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

        [HttpPut]
        [ProducesResponseType(typeof(SigningVM), 200)]
        public async Task<IActionResult> UpdateSigning(SigningVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signingService.UpdateSigning(viewmodel);
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

        [HttpGet]
        [ProducesResponseType(typeof(List<SigningVM>), 200)]
        public async Task<IActionResult> Get([FromQuery] bool isTemplate = true)
        {
            var response = await this._signingService.GetUserSignings(isTemplate);
            if (response == null)
            {
                return StatusCode(500, "We encountered and error trying to retrieve the signing records.");
            }

            return Ok(response);
        }

        [HttpGet("Card")]
        [ProducesResponseType(typeof(List<SigningVM>), 200)]
        public async Task<IActionResult> GetCards([FromQuery] bool isTemplate = true)
        {
            var response = await this._signingService.GetUserSigningsCards(isTemplate);
            if (response == null)
            {
                return StatusCode(500, "We encountered and error trying to retrieve the signing records.");
            }

            return Ok(response);
        }

        [HttpGet("PagedResult")]
        [ProducesResponseType(typeof(List<SigningVM>), 200)]
        public async Task<IActionResult> Get(int? page = 1, int pageSize = 10, string orderBy = nameof(Signing.CreatedBy), bool ascending = true)
        {
            var employees = await _signingService.PagedResult(orderBy, ascending, 1, 5);
            return Ok(employees);
        }

        [HttpGet("{signingId}")]
        [ProducesResponseType(typeof(SigningVM), 200)]
        public async Task<IActionResult> GetSigning(int signingId)
        {
            var response = await this._signingService.GetSigning(signingId);
            if (response == null)
            {
                return StatusCode(500, "We encountered and error trying to retrieve the signing record.");
            }
            return Ok(response);
        }

        [HttpPost("Recipient")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> AddRecipient(SigningRecipientMappingVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signingService.AddRecipient(viewmodel);
                if (response > 0)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, "Something went wrong");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{signingId}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteSigning(int signingId)
        {
            var response = await this._signingService.DeleteSigning(signingId);
            return Ok(response);
        }

        [HttpPost("ClientFields")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SaveClientFields(SigningClientInputVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._signingService.SaveClientFields(viewmodel);
                if (response)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, "Something went wrong");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("Page/{pageId}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeletePage(int pageId)
        {
            var response = await this._signingService.DeletePage(pageId);
            return Ok(response);
        }
    }
}
