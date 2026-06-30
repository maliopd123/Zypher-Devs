using Microsoft.EntityFrameworkCore;
using ProjetoChaves.Models;

namespace ProjetoChaves.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Chave> Chaves { get; set; }
        public DbSet<Negacao> Negacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de relacionamentos
            modelBuilder.Entity<Chave>()
                .HasOne(c => c.UsuarioResponsavel)
                .WithMany()
                .HasForeignKey(c => c.UsuarioResponsavelId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Negacao>()
                .HasOne(n => n.Usuario)
                .WithMany()
                .HasForeignKey(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Negacao>()
                .HasOne(n => n.Chave)
                .WithMany()
                .HasForeignKey(n => n.ChaveId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
