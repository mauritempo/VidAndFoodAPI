using Application.Interfaces;
using Application.Models.Response.Wines;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WineAndFoodAPI.Controllers.Wines
{
    [Route("api/[controller]")]
    [ApiController]
    public class WineController : ControllerBase
    {
        private readonly IWineService _wineService;

        public WineController(IWineService wineService)
        {
            _wineService = wineService;
        }

        [HttpGet("all-wines")] 
        public async Task<ActionResult<IEnumerable<WineListItemDto>>> GetAllWines()
        {
            var wines = await _wineService.GetAllWines();

            return Ok(wines);
        }

    }
}
