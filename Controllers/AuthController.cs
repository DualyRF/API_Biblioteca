using Microsoft.AspNetCore.Mvc;
using API_Biblioteca.Interfaces;
using API_Biblioteca.DTOs;

namespace API_Biblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Usuario y contraseña son requeridos"
                });
            }

            var result = await _authService.LoginAsync(request.Username, request.Password);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpGet("validate/{usuarioId}")]
        public async Task<ActionResult<bool>> ValidarUsuario(int usuarioId)
        {
            var isValid = await _authService.ValidarUsuarioAsync(usuarioId);
            return Ok(isValid);
        }
    }
}