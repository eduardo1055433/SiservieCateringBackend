using Microsoft.EntityFrameworkCore;
using SiservieCatering.API.Models.Tablas;

using SiservieCatering.API.Models.Tablas.General;

namespace SiservieCatering.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<VentaCab> VentasCab => Set<VentaCab>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<UsuarioEmpresa> UsuarioEmpresas => Set<UsuarioEmpresa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Clave compuesta para UsuarioEmpresa
        modelBuilder.Entity<UsuarioEmpresa>()
            .HasKey(ue => new { ue.UserId, ue.EmpSchema });
    }
}
