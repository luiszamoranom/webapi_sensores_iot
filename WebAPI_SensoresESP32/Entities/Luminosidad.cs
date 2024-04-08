using System.ComponentModel.DataAnnotations;

namespace WebAPI_SensoresESP32.Entities;

public class Luminosidad
{
    [Key]
    public Guid id { get; set; }
    
    public required double valor { get; set; }
    
    public DateTime createdAt { get; set; }
    
    public Luminosidad()
    {
        id = Guid.NewGuid();
        createdAt = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
            );
    }
}