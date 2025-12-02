using API_Biblioteca.Models;

namespace API_Biblioteca.DTOs
{
    public class PrestamoRequest
    {
        public int Id { get; set; }
        public string LibroIsbn { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
    }
}