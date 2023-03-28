using Microsoft.AspNetCore.Mvc;
using rapid_docs_services.Services.SurveyService;
using rapid_docs_services.Services.Survyeee;
using rapid_docs_viewmodels.ViewModels;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyeeController : ControllerBase
    {
        private readonly ISurveyeeService _surveyeeService;
        private readonly ISurveyService _surveyService;
        public SurveyeeController(ISurveyeeService surveyeeService, ISurveyService surveyService)
        {
            this._surveyeeService = surveyeeService;
            this._surveyService = surveyService;
        }

        [HttpPost("verify")]
        [ProducesResponseType(typeof(SigningVM), 200)]
        public async Task<IActionResult> VerifyAndGetSurvey(TokenVerificationVM tokenVM)
        {
            if (ModelState.IsValid)
            {
                var response = await this._surveyeeService.VerifyAndGetSurvey(tokenVM);
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

        [HttpPut("surveyFields")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SaveSurveyFields(SurveyClientInputVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._surveyService.SaveSurveyFields(viewmodel);
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
    }
}
