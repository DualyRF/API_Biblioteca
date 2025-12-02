namespace API_Biblioteca.DTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserData? User { get; set; }
        public string? Token { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}