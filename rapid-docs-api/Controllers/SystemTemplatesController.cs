using Microsoft.AspNetCore.Mvc;
using rapid_docs_api.Core;
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
    [VidaDocsAuthorize]
    public class SystemTemplatesController : ControllerBase
    {
        private readonly ISignerService _signerService;
        private readonly ISigningService _signingService;
        private readonly ISigningDocumentService _signingDocumentService;
        public SystemTemplatesController(ISignerService signerService, ISigningService signingService, ISigningDocumentService signingDocumentService)
        {
            this._signerService = signerService;
            this._signingService = signingService;
            this._signingDocumentService = signingDocumentService;
        }
    }
    }
