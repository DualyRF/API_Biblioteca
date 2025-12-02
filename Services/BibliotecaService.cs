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

        // ========== LIBROS ==========
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

        // ========== EMPLEADOS ==========
        public async Task<List<Usuario>> GetEmpleadosAsync()
        {
            return await _context.Usuarios
                .Where(u => u.Tipo != "Socio" && u.Activo)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }

        public async Task<Usuario?> GetEmpleadoByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Tipo != "Socio" && u.Activo);
        }

        public async Task<Usuario> CreateEmpleadoAsync(Usuario empleado)
        {
            empleado.Tipo = empleado.Tipo; // Administrador o Bibliotecario
            empleado.Activo = true;

            _context.Usuarios.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<Usuario?> UpdateEmpleadoAsync(int id, Usuario empleado)
        {
            var empleadoExistente = await _context.Usuarios.FindAsync(id);
            if (empleadoExistente == null)
                return null;

            empleadoExistente.Username = empleado.Username;
            empleadoExistente.Password = empleado.Password;
            empleadoExistente.Nombre = empleado.Nombre;
            empleadoExistente.Apellido = empleado.Apellido;
            empleadoExistente.Email = empleado.Email;
            empleadoExistente.Telefono = empleado.Telefono;
            empleadoExistente.Tipo = empleado.Tipo;
            empleadoExistente.Activo = empleado.Activo;


            await _context.SaveChangesAsync();
            return empleadoExistente;
        }
        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        // ========== SOCIOS ==========
        public async Task<List<Usuario>> GetSociosAsync()
        {
            return await _context.Usuarios
                .Where(u => u.Tipo == "Socio" && u.Activo)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }

        public async Task<Usuario?> GetSocioByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Tipo == "Socio" && u.Activo);
        }

        public async Task<Usuario> CreateSocioAsync(Usuario socio)
        {
            socio.Tipo = "Socio";
            socio.Activo = true;

            _context.Usuarios.Add(socio);
            await _context.SaveChangesAsync();
            return socio;
        }

        // ========== PRÉSTAMOS ==========
        public async Task<List<Prestamo>> GetPrestamosAsync()
        {
            return await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .Include(p => p.Empleado)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();
        }

        public async Task<Prestamo?> GetPrestamoByIdAsync(int id)
        {
            return await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .Include(p => p.Empleado)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Prestamo> CreatePrestamoAsync(PrestamoRequest prestamoRequest)
        {
            // Verificar que el libro existe y está disponible
            var libro = await _context.Libros.FindAsync(prestamoRequest.LibroIsbn);
            if (libro == null || libro.Estado != "Disponible")
                throw new Exception("Libro no disponible");

            // Verificar que el socio existe
            var socio = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == prestamoRequest.UsuarioId && u.Tipo == "Socio" && u.Activo);
            if (socio == null)
                throw new Exception("Socio no válido");

            // Verificar que el empleado existe
            var empleado = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == prestamoRequest.EmpleadoId && u.Tipo != "Socio" && u.Activo);
            if (empleado == null)
                throw new Exception("Empleado no válido");

            var prestamo = new Prestamo
            {
                LibroIsbn = prestamoRequest.LibroIsbn,
                UsuarioId = prestamoRequest.UsuarioId,
                EmpleadoId = prestamoRequest.EmpleadoId,
                FechaPrestamo = prestamoRequest.FechaPrestamo,
                FechaDevolucion = prestamoRequest.FechaDevolucion,
                Estado = "Activo",
                FechaRegistro = DateTime.UtcNow
            };

            // Actualizar estado del libro
            libro.Estado = "Prestado";

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return prestamo;
        }

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

        // Busquedas
        public async Task<List<Libro>> BuscarLibrosAsync(string criterio)
        {
            return await _context.Libros
                .Where(l => l.Titulo.Contains(criterio) ||
                           l.Autor.Contains(criterio) ||
                           l.Isbn.Contains(criterio))
                .OrderBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<List<Usuario>> BuscarSociosAsync(string criterio)
        {
            return await _context.Usuarios
                .Where(u => u.Tipo == "Socio" && u.Activo &&
                           (u.Nombre.Contains(criterio) ||
                            u.Apellido.Contains(criterio) ||
                            u.Curp.Contains(criterio)))
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }
    }
}