using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace appDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public IActionResult get()
        {
            return Ok("beta");
        }
    }
}
