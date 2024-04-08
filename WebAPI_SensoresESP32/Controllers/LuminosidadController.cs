using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_SensoresESP32.Context;
using WebAPI_SensoresESP32.Entities;

namespace WebAPI_SensoresESP32.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LuminosidadController : Controller
{
    private readonly MysqlContext _mysqlContext;

    public LuminosidadController(MysqlContext mysqlContext)
    {
        _mysqlContext = mysqlContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Luminosidad>>> getAllLuminosidades()
    {
        var luminosidades = await _mysqlContext.luminosidad
            .OrderByDescending(t => t.createdAt)
            .ToListAsync();
        
        
        return Ok(luminosidades);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<Luminosidad>> getLuminosidadById(int id)
    {
        var luminosidad = await _mysqlContext.luminosidad.FindAsync(id);
        if (luminosidad is null)
        {
            return NotFound("Luminosidad no encontrada con ese id");
        }
        return Ok(luminosidad);
    }

    [HttpPost]
    public async Task<ActionResult<Luminosidad>> addLuminosidad(Luminosidad luminosidad)
    {
        var existeLuminosidad = await _mysqlContext.luminosidad.FindAsync(luminosidad.id);
        if (existeLuminosidad != null)
        {
            return Conflict("Ya existe una luminosidad con ese id");
        }

        _mysqlContext.luminosidad.Add(luminosidad);
        await _mysqlContext.SaveChangesAsync();

        return Ok(luminosidad);
    }

    [HttpGet("promedio")]
    public async Task<ActionResult<double>> getPromedioLuminosidad()
    {
        var luminosidades = await _mysqlContext.luminosidad.ToListAsync();
        if (!luminosidades.Any())
        {
            return NotFound("No hay registros de luminosidad disponibles.");
        }

        var promedio = await _mysqlContext.luminosidad.AverageAsync(t => t.valor);
        return Ok(promedio);
    }

    [HttpGet("mas_reciente")]
    public async Task<ActionResult<Luminosidad>> getLuminosidadMasReciente()
    {
        var ultimaLuminosidad = await _mysqlContext.luminosidad
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
        var luminosidades = await _mysqlContext.luminosidad.ToListAsync();
        if (!luminosidades.Any())
        {
            return NotFound("No hay registros de luminosidad disponibles.");
        }

        var luminosidadMinima = await _mysqlContext.luminosidad.MinAsync(h => h.valor);
        return Ok(luminosidadMinima);
    }

    [HttpGet("maxima")]
    public async Task<ActionResult<double>> getLuminosidadMaxima()
    {
        var luminosidades = await _mysqlContext.luminosidad.ToListAsync();
        if (!luminosidades.Any())
        {
            return NotFound("No hay registros de luminosidad disponibles.");
        }

        var luminosidadMaxima = await _mysqlContext.luminosidad.MaxAsync(h => h.valor);
        return Ok(luminosidadMaxima);
    }
}
