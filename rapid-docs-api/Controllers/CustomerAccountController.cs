using Microsoft.AspNetCore.Mvc;
using rapid_docs_api.Core;
using rapid_docs_services.Services.CustomerAccount;
using rapid_docs_viewmodels.ViewModels;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [VidaDocsAuthorize]
    public class CustomerAccountController : ControllerBase
    {
        private readonly ICustomerAccountService _customerAccountService;
        public CustomerAccountController(ICustomerAccountService customerAccountService)
        {
            this._customerAccountService = customerAccountService;
        }

        [HttpPost("Logo")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SaveCompanyLogo([FromForm] CustomerAccountLogoVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var response = await this._customerAccountService.SaveCompanyLogo(viewmodel.Logo);
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
