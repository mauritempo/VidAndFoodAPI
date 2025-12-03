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
        private readonly IRatingService _ratingService;


        public WineController(IWineService wineService, IRatingService ratingService)
        {
            _wineService = wineService;
            _ratingService = ratingService;
        }

        [HttpGet("all-wines")] 
        public async Task<ActionResult<IEnumerable<WineListItemDto>>> GetAllWines()
        {
            var wines = await _wineService.GetAllWines();

            return Ok(wines);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WineByIdResponseDto>> GetWineById(Guid id)
        {
            try
            {
                var wineDetail = await _wineService.GetWineById(id);
                var ratingWine = await _ratingService.GetWineReviews(id);
                var response = new WineByIdResponseDto
                {
                    Wine = wineDetail,
                    Reviews = ratingWine
                };

                // 4. Retornamos el objeto completo
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("filters/wineries")] 
        public async Task<ActionResult<IEnumerable<string>>> GetWineries()
        {
            var wineries = await _wineService.GetAllWineries();
            return Ok(wineries);
        }

    }
}
