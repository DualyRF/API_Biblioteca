using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Biblioteca.Models
{
    public class Prestamo
    {
        [Key]
        [Column ("id")]
        public int Id { get; set; }

        [Required]
        [Column ("libroIsbn")]
        public string LibroIsbn { get; set; } = string.Empty;

        [Required]
        [Column ("usuarioId")]
        public int UsuarioId { get; set; }

        [Required]
        [Column ("fechaPrestamo")]
        public DateTime FechaPrestamo { get; set; }

        [Required]
        [Column ("fechaDevolucion")]
        public DateTime FechaDevolucion { get; set; }

        [Column ("fechaDevolucionReal")]
        public DateTime? FechaDevolucionReal { get; set; } // Puede ser nulo si no se ha devuelto el libro aún

        [Required]
        [Column ("estado")]
        public string Estado { get; set; } = "Activo"; // Por defecto es Activo

        [ForeignKey("libroIsbn")]
        public virtual Libro Libro { get; set; } = null!;

        [ForeignKey("usuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}