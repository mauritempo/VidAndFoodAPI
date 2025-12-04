using Application.Interfaces;
using Application.Models.Request.Wines;
using Application.Models.Response.Wines;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WineAndFoodAPI.Controllers.WineUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class WineUserController : ControllerBase
    {
        private readonly IWineUserService _wineUserService;
        private readonly IRatingService _ratingService;

        public WineUserController (IWineUserService wineUserService, IRatingService ratingService)
        {
            _wineUserService = wineUserService;
            _ratingService = ratingService;
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<WineListItemDto>>> GetHistory()
        {
            var history = await _wineUserService.GetHistoryList();
            return Ok(history);
        }

        [HttpGet("favorites")]
        public async Task<ActionResult<List<WineListItemDto>>> GetFavorites()
        {
            try
            {
                var favorites = await _wineUserService.ListFavoriteWines();
                return Ok(favorites);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPost("{wineId}/favorite")]
        public async Task<IActionResult> CreateFavourite(Guid wineId)
        {
            try
            {
                await _wineUserService.ToggleFavorite(wineId);
                return Ok(new { message = "Estado de favoritos actualizado." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{wineId}/history")]
        public async Task<IActionResult> RegisterConsumption(Guid wineId)
        {
            try
            {
                await _wineUserService.RegisterConsumption(wineId);
                return Ok(new { message = "Consumo registrado correctamente." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex) 
            {
                return StatusCode(409, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/favorite")]
        public async Task<IActionResult> RemoveFavorite(Guid id)
        {
            try
            {
                await _wineUserService.RemoveFavorite(id);
                return Ok("vino borrado de favoritos"); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/history")]
        public async Task<IActionResult> RemoveFromHistory(Guid id)
        {
            try
            {
                await _wineUserService.RemoveFromHistory(id);
                return Ok("vino borrado de historial");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
