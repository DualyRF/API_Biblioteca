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
                var nuevoPrestamo = await _bibliotecaService.CreatePrestamoAsync(prestamoRequest);
                return CreatedAtAction(nameof(GetPrestamo), new { id = nuevoPrestamo.Id }, nuevoPrestamo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear préstamo: {ex.Message}");
            }
        }

        [HttpPut("{id}/devolver")]
        public async Task<ActionResult<Prestamo>> RegistrarDevolucion(int id)
        {
            var prestamo = await _bibliotecaService.RegistrarDevolucionAsync(id);
            if (prestamo == null)
            {
                return NotFound($"Préstamo activo con ID {id} no encontrado");
            }

            return Ok(prestamo);
        }
    }
}