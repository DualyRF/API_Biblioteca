using API_Biblioteca.Models;
using API_Biblioteca.DTOs;

namespace API_Biblioteca.Interfaces
{
    public interface IBibliotecaService
    {
        // Libros
        Task<List<Libro>> GetLibrosAsync();
        Task<Libro?> GetLibroByIsbnAsync(string isbn);
        Task<Libro> CreateLibroAsync(Libro libro);
        Task<Libro?> UpdateLibroAsync(string isbn, Libro libro);
        Task<bool> DeleteLibroAsync(string isbn);

        // Empleados
        Task<List<Usuario>> GetUsuariosAsync();
        Task<Usuario?> GetUsuarioByIdAsync(int id);
        Task<Usuario> CreateUsuarioAsync(Usuario empleado);
        Task<Usuario?> UpdateUsuarioAsync(int id, Usuario empleado);
        Task<bool> DeleteEmpleadoAsync(int id);

        // Socios
        Task<List<Usuario>> GetSociosAsync();
        Task<Usuario?> GetSocioByIdAsync(int id);
        Task<Usuario> CreateSocioAsync(Usuario socio);

        // Préstamos
        Task<List<Prestamo>> GetPrestamosAsync();
        Task<Prestamo?> GetPrestamoByIdAsync(int id);
        Task<Prestamo> CreatePrestamoAsync(PrestamoRequest prestamoRequest);
        Task<Prestamo?> RegistrarDevolucionAsync(int prestamoId);

        // Búsquedas a la BD
        Task<List<Libro>> BuscarLibrosAsync(string criterio);
        Task<List<Usuario>> BuscarSociosAsync(string criterio);
    }
}