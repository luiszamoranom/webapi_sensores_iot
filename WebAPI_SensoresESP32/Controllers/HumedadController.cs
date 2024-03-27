using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_SensoresESP32.Context;
using WebAPI_SensoresESP32.Entities;

namespace WebAPI_SensoresESP32.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HumedadController: Controller
{
    private readonly InMemoryDatabaseContext _inMemoryDatabaseContext;

    public HumedadController(InMemoryDatabaseContext inMemoryDatabaseContext)
    {
        _inMemoryDatabaseContext = inMemoryDatabaseContext;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Humedad>>> getAllHumedades()
    {
        var humedades = await _inMemoryDatabaseContext.humedad
            .OrderByDescending(t => t.createdAt)
            .ToListAsync();
        return Ok(humedades);
    }
    
    [HttpGet("id/{id}")]
    public async Task<ActionResult<Humedad>> getHumedadById(Guid id)
    {
        var humedad = await _inMemoryDatabaseContext.humedad.FindAsync(id);
        if (humedad is null)
        {
            return NotFound("Humedad no encontrada ese id");
        }
        return Ok(humedad);
    }
    
    [HttpPost]
    public async Task<ActionResult<Humedad>> addHumedad(Humedad humedad)
    {
        var existeHumedad = await _inMemoryDatabaseContext.humedad.FindAsync(humedad.id);
        if (existeHumedad != null)
        {
            return Conflict("Ya existe una humedad con ese id");
        }
        
        _inMemoryDatabaseContext.humedad.Add(humedad);
        await _inMemoryDatabaseContext.SaveChangesAsync();

        return Ok(humedad);
    }
    
    [HttpGet("promedio")]
    public async Task<ActionResult<double>> getPromedioHumedad()
    {
        var promedio = await _inMemoryDatabaseContext.humedad.AverageAsync(t => t.valor);

        return Ok(promedio);
    }
    
    [HttpGet("mas_reciente")]
    public async Task<ActionResult<double>> getHumedadMasReciente()
    {
        var ultimaHumedad = await _inMemoryDatabaseContext.humedad
            .OrderByDescending(t => t.createdAt)
            .FirstOrDefaultAsync();

        if (ultimaHumedad == null)
        {
            return NotFound("No se encontraron registros de humedad.");
        }

        return Ok(ultimaHumedad);
    }
    
    [HttpGet("minima")]
    public async Task<ActionResult<double>> getHumedadMinima()
    {
        var humedadMinima = await _inMemoryDatabaseContext.humedad.MinAsync(h => h.valor);

        return Ok(humedadMinima);
    }
    
    [HttpGet("maxima")]
    public async Task<ActionResult<double>> getHumedadMaxima()
    {
        var humedadMinima = await _inMemoryDatabaseContext.humedad.MaxAsync(h => h.valor);

        return Ok(humedadMinima);
    }
    
    
}