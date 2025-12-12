using Application.Interfaces;
using Application.Models.Request.Auth;
using Application.Models.Request.User;
using Application.Models.Response.User;
using Application.Services;
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("all")] 
        public async Task<ActionResult<List<UserProfileDto>>> GetAllUsers()
        {
            try
            {
                var users = await _service.GetAllUsersAsync();
                return Ok(users);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Retorna 403 Forbidden si no es Admin
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("upgrade-to-sommelier")]
        public async Task<IActionResult> UpgradeToSommelier()
        {
            try
            {
                await _service.UpgradeToSommelierAsync();
                return Ok(new { message = "¡Felicidades! Tu cuenta ha sido actualizada a Sommelier." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // 409 Conflict o 400 Bad Request
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al actualizar el rol: " + ex.Message });
            }
        }

        [HttpPut("downgrade-to-user")]
        [Authorize] // Asegúrate de que solo usuarios logueados entren aquí
        public async Task<IActionResult> DowngradeToUser()
        {
            try
            {
                await _service.DownGradeToUserAsync();

                // IMPORTANTE: El mensaje avisa al frontend que debe hacer algo
                return Ok(new
                {
                    message = "Suscripción cancelada exitosamente. Tu cuenta ha vuelto al plan Básico.",
                    // Un flag útil para que tu Frontend sepa que debe forzar logout o refrescar el token
                    requiresLogout = true
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Por si un Admin intenta bajarse o si ya es usuario base
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al procesar la baja: " + ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize] // Obligatorio estar logueado
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _service.DeleteUserAsync(id);

                // Retornamos un mensaje claro.
                // Si el usuario se borró a sí mismo, el frontend debe recibir esto 
                // y forzar un Logout inmediato.
                return Ok(new { message = "Usuario y todos sus datos relacionados han sido eliminados permanentemente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // 403 Forbidden: Intentó borrar a otro sin ser Admin
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // 400 Bad Request: Reglas de negocio (ej. borrar un admin protegido)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al eliminar usuario: " + ex.Message });
            }
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

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
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
