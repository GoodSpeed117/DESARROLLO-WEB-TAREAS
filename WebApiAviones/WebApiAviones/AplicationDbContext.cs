using Microsoft.EntityFrameworkCore;
using WebApiAviones.Entidades;

namespace WebApiAviones
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Avion> Aviones { get; set; }
        public DbSet<Clase> Clases { get; set; }

    }
}
