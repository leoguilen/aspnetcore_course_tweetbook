using Microsoft.AspNetCore.Mvc;

namespace TweetBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Nome = "Leonardo" });
        }
    }
}