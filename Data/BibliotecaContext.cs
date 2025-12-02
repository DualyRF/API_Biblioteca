using Microsoft.EntityFrameworkCore;
using API_Biblioteca.Models;

namespace API_Biblioteca.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Curp).IsUnique();
                entity.HasIndex(u => u.Tipo);
                entity.HasIndex(u => u.Activo);
            });

            // Configurar Libro
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.HasIndex(l => l.Estado);
                entity.HasIndex(l => l.Autor);
            });

            // Configurar Prestamo
            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.HasIndex(p => p.Estado);
                entity.HasIndex(p => new { p.FechaPrestamo, p.FechaDevolucion });

                // Relaciones (configurar la FK en pocas palabras)
                entity.HasOne(p => p.Libro)
                      .WithMany(l => l.Prestamos)
                      .HasForeignKey(p => p.LibroIsbn)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Usuario)
                      .WithMany(u => u.PrestamosComoUsuario)
                      .HasForeignKey(p => p.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Empleado)
                      .WithMany(u => u.PrestamosComoEmpleado)
                      .HasForeignKey(p => p.EmpleadoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}