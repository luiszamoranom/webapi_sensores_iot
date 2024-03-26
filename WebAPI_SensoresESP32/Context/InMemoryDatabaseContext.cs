using Microsoft.EntityFrameworkCore;
using WebAPI_SensoresESP32.Entities;

namespace WebAPI_SensoresESP32.Context;

public class InMemoryDatabaseContext: DbContext
{
    public InMemoryDatabaseContext(DbContextOptions<InMemoryDatabaseContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Configuración para usar la base de datos en memoria
            optionsBuilder.UseInMemoryDatabase("WebAPI_SensoresESP32");
        }
    }
    
    public DbSet<Humedad> humedad { get; set; }
    public DbSet<Luminosidad> luminosidad { get; set; }
    public DbSet<Temperatura> temperatura { get; set; }
}