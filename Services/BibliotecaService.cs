using Microsoft.EntityFrameworkCore;
using API_Biblioteca.Data;
using API_Biblioteca.Interfaces;
using API_Biblioteca.Models;
using API_Biblioteca.DTOs;

namespace API_Biblioteca.Services
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly BibliotecaContext _context;

        public BibliotecaService(BibliotecaContext context)
        {
            _context = context;
        }

        // ========== Libros ==========
        public async Task<List<Libro>> GetLibrosAsync()
        {
            return await _context.Libros
                .OrderBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<Libro?> GetLibroByIsbnAsync(string isbn)
        {
            return await _context.Libros
                .FirstOrDefaultAsync(l => l.Isbn == isbn);
        }

        public async Task<Libro> CreateLibroAsync(Libro libro)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();
            return libro;
        }

        public async Task<Libro?> UpdateLibroAsync(string isbn, Libro libro)
        {
            var libroExistente = await _context.Libros.FindAsync(isbn);
            if (libroExistente == null) return null;

            libroExistente.Titulo = libro.Titulo;
            libroExistente.Autor = libro.Autor;
            libroExistente.Editorial = libro.Editorial;
            libroExistente.AnioPublicacion = libro.AnioPublicacion;
            libroExistente.Genero = libro.Genero;
            libroExistente.Estado = libro.Estado;

            await _context.SaveChangesAsync();
            return libroExistente;
        }

        public async Task<bool> DeleteLibroAsync(string isbn)
        {
            var libro = await _context.Libros.FindAsync(isbn);
            if (libro == null) return false;

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            return true;
        }

        // ========== Usuarios ==========
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> UpdateUsuarioAsync(int id, Usuario usuario)
        {
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuarioExistente == null)
                return null;

            usuarioExistente.Username = usuario.Username;
            usuarioExistente.Password = usuario.Password;
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.Tipo = usuario.Tipo;
            usuarioExistente.CURP = usuario.CURP;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Telefono = usuario.Telefono;
            usuarioExistente.Activo = usuario.Activo;
            _context.Usuarios.Update(usuarioExistente);
            await _context.SaveChangesAsync();
            return usuarioExistente;
        }
        
        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        // ========== Prestamo ==========
        public async Task<List<Prestamo>> GetPrestamosAsync()
        {
            return await _context.Prestamo
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();
        }
        

        public async Task<Prestamo?> GetPrestamoByIdAsync(int id)
        {
            return await _context.Prestamo
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PrestamoRequest> CreatePrestamoAsync(PrestamoRequest prestamoRequest)
        {
            // Verificar que el libro existe y está disponible
            var libro = await _context.Libros.FindAsync(prestamoRequest.LibroIsbn);
            if (libro == null || libro.Estado != "Disponible")
                throw new Exception("Libro no disponible");

            // Verificar que el socio existe
            var socio = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == prestamoRequest.UsuarioId && u.Tipo == "Socio" && u.Activo == 1);
            if (socio == null)
                throw new Exception("Socio no válido");

            var prestamo = new PrestamoRequest
            {
                LibroIsbn = prestamoRequest.LibroIsbn,
                UsuarioId = prestamoRequest.UsuarioId,
                FechaPrestamo = prestamoRequest.FechaPrestamo,
                FechaDevolucion = prestamoRequest.FechaDevolucion
            };

            // Actualizar estado del libro
            libro.Estado = "Prestado";

            _context.PrestamosRequest.Add(prestamo);
            await _context.SaveChangesAsync();

            return prestamo;
        }

        /*
        public async Task<Prestamo?> RegistrarDevolucionAsync(int prestamoId)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.Id == prestamoId && p.Estado == "Activo");

            if (prestamo == null) return null;

            prestamo.Estado = "Completado";
            prestamo.FechaDevolucionReal = DateTime.UtcNow;

            // Liberar el libro
            prestamo.Libro.Estado = "Disponible";

            await _context.SaveChangesAsync();
            return prestamo;
        }

        */
    }
}