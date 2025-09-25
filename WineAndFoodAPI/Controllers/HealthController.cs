using Microsoft.AspNetCore.Mvc;

namespace WineAndFoodAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok("Healthy");
    }
}
