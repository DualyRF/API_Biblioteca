using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Biblioteca.Models
{
    public class Libro
    {
        [Key]
        [Column ("isbn")]
        [StringLength (20)]
        public string? Isbn { get; set; }

        [Required]
        [Column ("titulo")]
        [StringLength(200)]
        public string? Titulo { get; set; }

        [Required]
        [Column ("autor")]
        [StringLength(100)]
        public string? Autor { get; set; }

        [Column ("editorial")]
        [StringLength(100)]
        public string? Editorial { get; set; }

        [Column ("anioPublicacion")]
        public int? AnioPublicacion { get; set; }

        [Column ("genero")]
        [StringLength(50)]
        public string? Genero { get; set; }

        [Required]
        [Column ("estado")]
        [StringLength(20)]
        public string Estado { get; set; } = "Disponible"; // Por defecto es Disponible
    }
}