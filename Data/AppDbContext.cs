using Microsoft.EntityFrameworkCore;
using SiservieCatering.API.Models.Tablas;

namespace SiservieCatering.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<VentaCab> VentasCab => Set<VentaCab>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
