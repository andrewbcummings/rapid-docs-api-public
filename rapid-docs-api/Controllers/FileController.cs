using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rapid_docs_models.DbModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [Route("[controller]")]
    public class FileController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(File), 200)]
        public IActionResult Get(Guid fileName)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }
    }
}
