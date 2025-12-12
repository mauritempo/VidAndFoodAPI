using Application.Interfaces;
using Application.Models.Request.Wines;
using Application.Models.Response.Wines;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("admin-create")]
        [Authorize] // Asegura que esté logueado (el servicio valida si es Admin)
        public async Task<IActionResult> CreateWine([FromBody] CreateWineRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newWine = await _wineService.CreateWine(request);

                // Retornamos 201 Created con la URL para ver el vino
                // Asumiendo que tienes un método llamado "GetWineById" en este controlador
                return CreatedAtAction(nameof(GetWineById), new { id = newWine.Id }, newWine);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno: " + ex.Message });
            }
        }

        // ==========================================================
        // UPDATE WINE (ADMIN)
        // ==========================================================
        [HttpPut("admin-update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateWine(Guid id, [FromBody] UpdateWineRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedWine = await _wineService.UpdateWine(id, request);
                return Ok(updatedWine);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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

        // ==========================================================
        // SOFT DELETE WINE (ADMIN)
        // ==========================================================
        [HttpDelete("admin-delete/{id}")]
        [Authorize]
        public async Task<IActionResult> SoftDeleteWine(Guid id)
        {
            try
            {
                await _wineService.SoftDeleteWine(id);

                // Opción A: Devolver 200 OK con mensaje
                return Ok(new { message = "Vino eliminado (soft delete) correctamente." });

                // Opción B: Devolver 204 No Content (Estándar REST, sin cuerpo)
                // return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
