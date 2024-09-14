using Microsoft.EntityFrameworkCore;

namespace APIUsuarios.Models
{
    public class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext(DbContextOptions<UsuariosDBContext> options)
        : base(options)
        {
        }

        public DbSet<UsuarioT> UsuariosT { get; set; } = null;
        public DbSet<RolT> RolesT { get; set; } = null;
        public DbSet<PermisoT> PermisosT { get; set; } = null;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
