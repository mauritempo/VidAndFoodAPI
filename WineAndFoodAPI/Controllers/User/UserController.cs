using Application.Interfaces;
using Application.Models.Request.Auth;
using Application.Models.Request.User;
using Application.Models.Response.User;
using Application.Services;
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;


namespace WineAndFoodAPI.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IAuthentication _authService;
        public UserController(IUserService service, IAuthentication authentication)
        {
            _service = service;
            _authService = authentication;
        }


        [HttpGet("{id}/asd")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("{id}")] // GET: api/users/d290f1ee-6c54...
        public async Task<ActionResult<UserProfileDto>> GetUserById(Guid id)
        {
            try
            {
                var userProfile = await _service.GetUserByIdAsync(id);
                return Ok(userProfile);
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

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userForCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _service.CreateUserAsync(userForCreate);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authResponse = await _authService.LoginAsync(loginRequest);
            if (authResponse == null)
                return Unauthorized("Invalid email or password.");
            return Ok(authResponse);
        }

        //[HttpPost("seed-admin")]
        //public async Task<IActionResult> CreateAdminSeed(
        //[FromBody] CreateAdminRequest request,
        //[FromHeader] string secretKey) // Recibimos el header aquí
        //    {
        //        // Le pasamos la clave al servicio. Si está mal, el servicio explotará con una excepción.
        //        var result = await _service.CreateAdminForceAsync(request);
        //        return Ok(result);
        //    }

    }
}
