using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rapid_docs_api.Core;
using rapid_docs_services.Services.Account;
using rapid_docs_viewmodels.ViewModels;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [VidaDocsAuthorize]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpPost]
        [Route("UserDetails")]
        public async Task<IActionResult> UserDetails(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await this._accountService.UserDetails(userViewModel);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
