using Microsoft.AspNetCore.Mvc;
using API_Biblioteca.Interfaces;
using API_Biblioteca.Models;

namespace API_Biblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IBibliotecaService _bibliotecaService;


        public UsuarioController(IBibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        // Toma todos los usuarios de la base de datos
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            var empleados = await _bibliotecaService.GetUsuariosAsync();
            return Ok(empleados);
        }

        // Toma un usuario por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var empleado = await _bibliotecaService.GetUsuarioByIdAsync(id);
            if (empleado == null)
            {
                return NotFound($"Empleado con ID {id} no encontrado");
            }
            return Ok(empleado);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario request)
        {
            try
            {
                // Crea un nuevo usuario basado en el request recibido
                var usuario = new Usuario
                {
                    Username = request.Username,
                    Password = request.Password,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Tipo = request.Tipo,
                    CURP = request.CURP,
                    Email = request.Email,
                    Telefono = request.Telefono
                };

                // Llama al servicio para crear el usuario
                var nuevoUsuario = await _bibliotecaService.CreateUsuarioAsync(usuario);
                return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear empleado: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> UpdateUsuario(int id, Usuario empleado)
        {
            var empleadoActualizado = await _bibliotecaService.UpdateUsuarioAsync(id, empleado);
            if (empleadoActualizado == null)
            {
                return NotFound($"Empleado con ID {id} no encontrado");
            }

            return Ok(empleadoActualizado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmpleado(int id)
        {
            var eliminado = await _bibliotecaService.DeleteEmpleadoAsync(id);
            if (!eliminado)
            {
                return NotFound($"Empleado con ID {id} no encontrado");
            }
            return NoContent();
        }
    }
}