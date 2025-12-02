using System.ComponentModel.DataAnnotations;

namespace API_Biblioteca.Models
{
    public class Libro
    {
        [Key]
        [StringLength(20)]
        public string Isbn { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Autor { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Editorial { get; set; }

        public int? AnioPublicacion { get; set; }

        [StringLength(50)]
        public string? Genero { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Disponible";

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}