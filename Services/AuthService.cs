using Microsoft.EntityFrameworkCore;
using API_Biblioteca.Data;
using API_Biblioteca.Interfaces;
using API_Biblioteca.Models;
using API_Biblioteca.DTOs;

namespace API_Biblioteca.Services
{
    public class AuthService : IAuthService
    {
        private readonly BibliotecaContext _context;

        public AuthService(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                // Buscar usuario por username (solo empleados tienen username)
                var usuario = await _context.Usuarios
                    .Where(u => u.Username == username &&
                               u.Tipo != "Socio" &&
                               u.Activo)
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    };
                }

                // Verificar contraseña (hash MD5 simple para el ejemplo)
                if (usuario.Password != password)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Contraseña incorrecta"
                    };
                }

                // Login exitoso
                return new LoginResponse
                {
                    Success = true,
                    Message = "Login exitoso",
                    User = new UserData
                    {
                        Id = usuario.Id,
                        Username = usuario.Username!,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Tipo = usuario.Tipo,
                        Email = usuario.Email ?? ""
                    },
                    Token = GenerateSimpleToken(usuario.Id)
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<bool> ValidarUsuarioAsync(int usuarioId)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Id == usuarioId && u.Activo);
        }

        public async Task<Usuario?> GetUsuarioByUsernameAsync(string username)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Activo);
        }

        private string CreateMD5Hash(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes).ToLower();
            }
        }

        private string GenerateSimpleToken(int userId)
        {
            return $"token_{userId}_{DateTime.Now.Ticks}";
        }
    }
}