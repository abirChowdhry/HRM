using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    // Reserved for future report/download endpoints.
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PdfReportController : ControllerBase
    {
    }
}
