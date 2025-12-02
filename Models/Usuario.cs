using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Biblioteca.Models
{
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        [Column("password")]
        [StringLength(50)]
        public string Password { get; set; } = string.Empty;
        [Column("nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [Column("apellido")]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;
        [Column("tipo")]
        [StringLength(100)]
        public string Tipo { get; set; } = string.Empty;
        [Column("curp")]
        [StringLength(18)]
        public string CURP { get; set; } = string.Empty;
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [Column("telefono")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;
        [Column("activo")]
        public int? Activo { get; set; } = 1; // Por defecto es activo (1)
    }
}