using Microsoft.AspNetCore.Mvc;
using API_Biblioteca.Interfaces;
using API_Biblioteca.Models;
using API_Biblioteca.DTOs;

namespace API_Biblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly IBibliotecaService _bibliotecaService;
        public PrestamosController(IBibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Prestamo>>> GetPrestamos()
        {
            var prestamos = await _bibliotecaService.GetPrestamosAsync();
            return Ok(prestamos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
        {
            var prestamo = await _bibliotecaService.GetPrestamoByIdAsync(id);
            if (prestamo == null)
            {
                return NotFound($"Préstamo con ID {id} no encontrado");
            }
            return Ok(prestamo);
        }
        [HttpPost]
        public async Task<ActionResult<Prestamo>> CreatePrestamo(PrestamoRequest prestamoRequest)
        {
            try
            {
                if (prestamoRequest.FechaDevolucion <= prestamoRequest.FechaPrestamo)
                    return BadRequest("La fecha de devolución debe ser posterior a la fecha de préstamo");

                var nuevoPrestamo = await _bibliotecaService.CreatePrestamoAsync(prestamoRequest);

                return CreatedAtAction(nameof(GetPrestamo), new { id = nuevoPrestamo.Id }, nuevoPrestamo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear préstamo: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Prestamo>> UpdatePrestamo(int id, Prestamo prestamo)
        {
            if (prestamo.FechaDevolucion <= prestamo.FechaPrestamo)
                return BadRequest("La fecha de devolución debe ser posterior a la fecha de préstamo");
            var prestamoActualizado = await _bibliotecaService.UpdatePrestamoAsync(id, prestamo);
            if (prestamoActualizado == null)
            {
                return NotFound($"Préstamo con ID {id} no encontrado");
            }
            return Ok(prestamoActualizado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmpleado(int id)
        {
            var eliminado = await _bibliotecaService.DeletePrestamoAsync(id);
            if (!eliminado)
            {
                return NotFound($"Prestamo con ID {id} no encontrado");
            }
            return NoContent();
        }

    }
}