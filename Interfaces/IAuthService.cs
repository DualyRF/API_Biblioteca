using API_Biblioteca.DTOs;
using API_Biblioteca.Models;

namespace API_Biblioteca.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string username, string password);
        Task<bool> ValidarUsuarioAsync(int usuarioId);
        Task<Usuario?> GetUsuarioByUsernameAsync(string username);
    }
}