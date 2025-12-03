using Application.Interfaces;
using Application.Models.Response.Wines;
using Application.Services;
using Domain.Model.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WineAndFoodAPI.Controllers.GrapeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrapeController : ControllerBase
    {
        private readonly IGrapeService _grapeService;
        public GrapeController(IGrapeService grapeService) 
        { 
               _grapeService = grapeService;
        }

        [HttpGet("all-grapes")]
        public async Task<ActionResult<IEnumerable<GrapeDto>>> GetAllGrapes()
        {
            var grapes = await _grapeService.GetAllGrapes();

            return Ok(grapes);
        }

    }
}
