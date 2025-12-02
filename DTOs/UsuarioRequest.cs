namespace API_Biblioteca.DTOs
{
    public class UsuarioRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Curp { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}