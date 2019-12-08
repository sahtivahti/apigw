using Microsoft.AspNetCore.Mvc;

namespace apigw.Controllers
{
    public class HealthController : Controller
    {
        [HttpGet("/health")]
        public OkObjectResult HealthAction()
        {
            return Ok("OK");
        }
    }
}
