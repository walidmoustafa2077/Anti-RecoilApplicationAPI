using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anti_RecoilApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/home
        [HttpGet]
        public IActionResult GetWelcomeMessage()
        {
            return Ok(new { Message = "Welcome to the Anti-Recoil Application API!" });
        }

        // POST: api/home
        [Authorize]
        [HttpPost]
        public IActionResult PostData([FromBody] object data)
        {
            // Example processing of incoming data
            return Ok(new { Message = "Data received successfully", ReceivedData = data });
        }
    }
}
