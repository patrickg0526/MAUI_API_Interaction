using ItemsWebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ItemsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET: api/Auth - returns 200 only when valid Basic Authentication credentials are supplied
        [HttpGet]
        [BasicAuthentication]
        public IActionResult Get()
        {
            return Ok(new { message = "Authenticated" });
        }
    }
}
