using Microsoft.AspNetCore.Mvc;
using rapid_docs_services.Services.System;
using System;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ISystemService _systemService;

        public SystemController(ISystemService systemService)
        {
            this._systemService = systemService;
        }

        [HttpDelete("CleanSignings")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CleanSignings()
        {
            try
            {
                await this._systemService.CleanSignings();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("CleanBlobStorage")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CleanBlobStorage()
        {
            try
            {
                await this._systemService.CleanBlobStorage();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
