using Microsoft.AspNetCore.Mvc;

namespace rapid_docs_api.Controllers
{
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Health Check complete. Api Started.");
        }
    }
}
