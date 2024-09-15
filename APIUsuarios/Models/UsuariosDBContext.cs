using Microsoft.EntityFrameworkCore;

namespace APIUsuarios.Models
{
    public class UsuariosDBContext : DbContext
    {
        public UsuariosDBContext(DbContextOptions<UsuariosDBContext> options)
        : base(options)
        {
        }

        public DbSet<UsuarioT> UsuariosT { get; set; } 
        public DbSet<RolT> RolesT { get; set; } 
        public DbSet<PermisoT> PermisosT { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<UsuarioT>()
            //        .HasOne(u => u.Rol)  
            //        .WithMany(r => r.Usuarios)
            //        .HasForeignKey(u => u.RolId);
        }
    }
}
