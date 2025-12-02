namespace API_Biblioteca.DTOs
{
    public class PrestamoRequest
    {
        public string LibroIsbn { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
    }
}