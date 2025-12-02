using Microsoft.EntityFrameworkCore;
using API_Biblioteca.Models;
using API_Biblioteca.DTOs;

namespace API_Biblioteca.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<PrestamoRequest> Prestamos { get; set; }
        public DbSet<Prestamo> Prestamo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.CURP).IsUnique();
                entity.HasIndex(u => u.Tipo);
                entity.HasIndex(u => u.Activo);
                entity.HasIndex(u => u.Email).IsUnique();
            });

        }
    }
}