using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_SensoresESP32.Context;
using WebAPI_SensoresESP32.Entities;

namespace WebAPI_SensoresESP32.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LuminosidadController: Controller
{
    private readonly InMemoryDatabaseContext _inMemoryDatabaseContext;

    public LuminosidadController(InMemoryDatabaseContext inMemoryDatabaseContext)
    {
        _inMemoryDatabaseContext = inMemoryDatabaseContext;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Luminosidad>>> getAllLuminosidades()
    {
        var luminosidades = await _inMemoryDatabaseContext.luminosidad.ToListAsync();
        return Ok(luminosidades);
    }
    
    [HttpGet("id/{id}")]
    public async Task<ActionResult<Luminosidad>> getLuminosidadById(Guid id)
    {
        var luminosidad = await _inMemoryDatabaseContext.luminosidad.FindAsync(id);
        if (luminosidad is null)
        {
            return NotFound("Luminosidad no encontrada ese id");
        }
        return Ok(luminosidad);
    }
    
    [HttpPost]
    public async Task<ActionResult<Luminosidad>> addLuminosidad(Luminosidad luminosidad)
    {
        var existeLuminosidad = await _inMemoryDatabaseContext.temperatura.FindAsync(luminosidad.id);
        if (existeLuminosidad != null)
        {
            return Conflict("Ya existe una luminosidad con ese id");
        }
        
        _inMemoryDatabaseContext.luminosidad.Add(luminosidad);
        await _inMemoryDatabaseContext.SaveChangesAsync();

        return Ok(luminosidad);
    }
    
    [HttpGet("promedio")]
    public async Task<ActionResult<double>> getPromedioLuminosidad()
    {
        var promedio = await _inMemoryDatabaseContext.luminosidad.AverageAsync(t => t.valor);

        return Ok(promedio);
    }
    
    [HttpGet("mas_reciente")]
    public async Task<ActionResult<double>> getLuminosidadMasReciente()
    {
        var ultimaLuminosidad = await _inMemoryDatabaseContext.luminosidad
            .OrderByDescending(t => t.createdAt)
            .FirstOrDefaultAsync();

        if (ultimaLuminosidad == null)
        {
            return NotFound("No se encontraron registros de luminosidad.");
        }

        return Ok(ultimaLuminosidad);
    }
    
    [HttpGet("minima")]
    public async Task<ActionResult<double>> getLuminosidadMinima()
    {
        var luminosidadMinima = await _inMemoryDatabaseContext.luminosidad.MinAsync(h => h.valor);

        return Ok(luminosidadMinima);
    }
    
    [HttpGet("maxima")]
    public async Task<ActionResult<double>> getLuminosidadMaxima()
    {
        var luminosidadMaxima = await _inMemoryDatabaseContext.luminosidad.MaxAsync(h => h.valor);

        return Ok(luminosidadMaxima);
    }
}