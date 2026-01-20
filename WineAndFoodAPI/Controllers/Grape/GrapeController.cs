using Application.Interfaces;
using Application.Models.Request.Grape; // Asegúrate que el namespace coincida con tu carpeta
using Application.Models.Response;
using Application.Models.Response.Wines; // O donde tengas GrapeDto
using Domain.Model.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WineAndFoodAPI.Controllers.GrapeController
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class GrapeController : ControllerBase
    {
        private readonly IGrapeService _grapeService;

        public GrapeController(IGrapeService grapeService)
        {
            _grapeService = grapeService;
        }

        [HttpGet("all-grapes")]
        public async Task<ActionResult<IEnumerable<GrapeResponseDto>>> GetAllGrapes()
        {
            var grapes = await _grapeService.GetAllGrapes();
            return Ok(grapes);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGrape([FromBody] NewGrape newGrape)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _grapeService.CreateGrape(newGrape);

                return StatusCode(201, new { message = "Uva creada exitosamente" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex) // Para nombres duplicados
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno: " + ex.Message });
            }
        }

        [HttpPut("update/{id}")] // Es importante recibir el ID en la URL
        public async Task<IActionResult> UpdateGrape(Guid id, [FromBody] UpdateGrapeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _grapeService.UpdateGrape(id, request);
                return Ok(new { message = "Uva actualizada correctamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex) // Para duplicados de nombre
            {
                return Conflict(new { message = ex.Message }); // 409 Conflict es ideal para duplicados
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ==========================================================
        // DELETE (DELETE)
        // ==========================================================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGrape(Guid id)
        {
            try
            {
                await _grapeService.DeleteGrape(id);
                return Ok(new { message = "Uva eliminada correctamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex) // Si tiene vinos relacionados
            {
                // 400 Bad Request o 409 Conflict son apropiados aquí
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}