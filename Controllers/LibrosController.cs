using Microsoft.AspNetCore.Mvc;
using API_Biblioteca.Interfaces;
using API_Biblioteca.Models;

namespace API_Biblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly IBibliotecaService _bibliotecaService;

        public LibrosController(IBibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibros()
        {
            var libros = await _bibliotecaService.GetLibrosAsync();
            return Ok(libros);
        }

        [HttpGet("{isbn}")]
        public async Task<ActionResult<Libro>> GetLibro(string isbn)
        {
            var libro = await _bibliotecaService.GetLibroByIsbnAsync(isbn);
            if (libro == null)
            {
                return NotFound($"Libro con ISBN {isbn} no encontrado");
            }
            return Ok(libro);
        }

        [HttpPost]
        public async Task<ActionResult<Libro>> CreateLibro(Libro libro)
        {
            try
            {
                var nuevoLibro = await _bibliotecaService.CreateLibroAsync(libro);
                return CreatedAtAction(nameof(GetLibro), new { isbn = nuevoLibro.Isbn }, nuevoLibro);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear libro: {ex.Message}");
            }
        }

        [HttpPut("{isbn}")]
        public async Task<ActionResult<Libro>> UpdateLibro(string isbn, Libro libro)
        {
            var libroActualizado = await _bibliotecaService.UpdateLibroAsync(isbn, libro);
            if (libroActualizado == null)
            {
                return NotFound($"Libro con ISBN {isbn} no encontrado");
            }

            return Ok(libroActualizado);
        }

        [HttpDelete("{isbn}")]
        public async Task<ActionResult> DeleteLibro(string isbn)
        {
            var eliminado = await _bibliotecaService.DeleteLibroAsync(isbn);
            if (!eliminado)
            {
                return NotFound($"Libro con ISBN {isbn} no encontrado");
            }

            return NoContent();
        }
    }
}