using Microsoft.AspNetCore.Mvc;
using API_Biblioteca.Interfaces;
using API_Biblioteca.Models;

namespace API_Biblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly IBibliotecaService _bibliotecaService;


        public EmpleadosController(IBibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetEmpleados()
        {
            var empleados = await _bibliotecaService.GetEmpleadosAsync();
            return Ok(empleados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetEmpleado(int id)
        {
            var empleado = await _bibliotecaService.GetEmpleadoByIdAsync(id);
            if (empleado == null)
            {
                return NotFound($"Empleado con ID {id} no encontrado");
            }
            return Ok(empleado);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateEmpleado(Usuario request)
        {
            try
            {
                var empleado = new Usuario
                {
                    Username = request.Username,
                    Password = request.Password,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Tipo = request.Tipo,
                    Curp = request.Curp,
                    Email = request.Email,
                    Telefono = request.Telefono,
                    Activo = true
                };

                var nuevoEmpleado = await _bibliotecaService.CreateEmpleadoAsync(empleado);
                return CreatedAtAction(nameof(GetEmpleado), new { id = nuevoEmpleado.Id }, nuevoEmpleado);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear empleado: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> UpdateEmpleado(int id, Usuario empleado)
        {
            var empleadoActualizado = await _bibliotecaService.UpdateEmpleadoAsync(id, empleado);
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