using System.ComponentModel.DataAnnotations;

namespace API_Biblioteca.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? Username { get; set; }

        [StringLength(255)]
        public string? Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [StringLength(18)]
        public string Curp { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        public bool Activo { get; set; } = true;
        public virtual ICollection<Prestamo> PrestamosComoUsuario { get; set; } = new List<Prestamo>();
        public virtual ICollection<Prestamo> PrestamosComoEmpleado { get; set; } = new List<Prestamo>();
    }
}