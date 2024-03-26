using System.ComponentModel.DataAnnotations;

namespace WebAPI_SensoresESP32.Entities;

public class Humedad
{
    [Key]
    public Guid id { get; set; }
    
    public required double valor { get; set; }
    
    public DateTime createdAt { get; set; }
    
    public Humedad()
    {
        id = Guid.NewGuid();
        createdAt = DateTime.UtcNow;
    }
}