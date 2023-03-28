using Microsoft.AspNetCore.Mvc;
using rapid_docs_api.Core;
using rapid_docs_services.Services.SurveyService;
using rapid_docs_viewmodels.ViewModels;
using System;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [VidaDocsAuthorize]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        public SurveyController(ISurveyService surveyService)
        {
            this._surveyService = surveyService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SurveyVM), 200)]
        public async Task<IActionResult> AddSurvey(SurveyVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._surveyService.AddSurvey(viewmodel);
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
        [ProducesResponseType(typeof(SurveyVM), 200)]
        public async Task<IActionResult> UpdateSurvey(SurveyVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._surveyService.UpdateSurvey(viewmodel);
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

        [HttpGet("{surveyId}")]
        [ProducesResponseType(typeof(SurveyVM), 200)]
        public async Task<IActionResult> GetSurvey(int surveyId)
        {
            var response = await this._surveyService.GetSurvey(surveyId);
            if (response == null)
            {
                return StatusCode(500, "We encountered and error trying to retrieve the signing record.");
            }
            return Ok(response);
        }

        [HttpDelete("Page/{pageId}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeletePage(int pageId)
        {
            var response = await this._surveyService.DeletePage(pageId);
            return Ok(response);
        }

        [HttpPost("Recipient")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> AddRecipient(SurveyRecipientMappingVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._surveyService.AddRecipient(viewmodel);
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
    }
}
