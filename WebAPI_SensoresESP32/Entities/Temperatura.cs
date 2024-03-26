using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_SensoresESP32.Entities;

public class Temperatura
{
    [Key]
    public Guid id { get; set; }
    
    public required double valor { get; set; }
    
    public DateTime createdAt { get; set; }
    
    public Temperatura()
    {
        id = Guid.NewGuid();
        createdAt = DateTime.UtcNow;
    }
}
