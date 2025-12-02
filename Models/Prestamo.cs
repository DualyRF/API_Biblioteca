using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Biblioteca.Models
{
    public class Prestamo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string LibroIsbn { get; set; } = string.Empty;

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        [Required]
        public DateTime FechaPrestamo { get; set; }

        [Required]
        public DateTime FechaDevolucion { get; set; }

        public DateTime? FechaDevolucionReal { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [ForeignKey("LibroIsbn")]
        public virtual Libro Libro { get; set; } = null!;

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey("EmpleadoId")]
        public virtual Usuario Empleado { get; set; } = null!;
    }
}