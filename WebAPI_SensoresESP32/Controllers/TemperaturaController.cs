using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_SensoresESP32.Context;
using WebAPI_SensoresESP32.Entities;

namespace WebAPI_SensoresESP32.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TemperaturaController: Controller
{
    private readonly InMemoryDatabaseContext _inMemoryDatabaseContext;

    public TemperaturaController(InMemoryDatabaseContext inMemoryDatabaseContext)
    {
        _inMemoryDatabaseContext = inMemoryDatabaseContext;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Temperatura>>> getAllTemperaturas()
    {
        var temperaturas = await _inMemoryDatabaseContext.temperatura.ToListAsync();
        return Ok(temperaturas);
    }
    
    [HttpGet("id/{id}")]
    public async Task<ActionResult<Temperatura>> getTemperaturaById(Guid id)
    {
        var temperatura = await _inMemoryDatabaseContext.temperatura.FindAsync(id);
        if (temperatura is null)
        {
            return NotFound("Temperatura no encontrada ese id");
        }
        return Ok(temperatura);
    }
    
    [HttpPost]
    public async Task<ActionResult<Temperatura>> addTemperatura(Temperatura temperatura)
    {
        var existeTemperatura = await _inMemoryDatabaseContext.temperatura.FindAsync(temperatura.id);
        if (existeTemperatura != null)
        {
            return Conflict("Ya existe una temperatura con ese id");
        }
        
        _inMemoryDatabaseContext.temperatura.Add(temperatura);
        await _inMemoryDatabaseContext.SaveChangesAsync();

        return Ok(temperatura);
    }
    
    [HttpGet("promedio")]
    public async Task<ActionResult<double>> getPromedioTemperatura()
    {
        var promedio = await _inMemoryDatabaseContext.temperatura.AverageAsync(t => t.valor);

        return Ok(promedio);
    }
    
    [HttpGet("mas_reciente")]
    public async Task<ActionResult<Temperatura>> getTemperaturaMasReciente()
    {
        var ultimaTemperatura = await _inMemoryDatabaseContext.temperatura
            .OrderByDescending(t => t.createdAt)
            .FirstOrDefaultAsync();

        if (ultimaTemperatura == null)
        {
            return NotFound("No se encontraron registros de temperatura.");
        }

        return Ok(ultimaTemperatura);
    }
    
    [HttpGet("minima")]
    public async Task<ActionResult<double>> getTemperaturaMinima()
    {
        var temperaturaMinima = await _inMemoryDatabaseContext.temperatura.MinAsync(h => h.valor);

        return Ok(temperaturaMinima);
    }
    
    [HttpGet("maxima")]
    public async Task<ActionResult<double>> getTemperaturaMaxima()
    {
        var temperaturaMaxima = await _inMemoryDatabaseContext.temperatura.MaxAsync(h => h.valor);

        return Ok(temperaturaMaxima);
    }
}